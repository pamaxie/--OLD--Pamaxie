using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CoenM.ImageHash;
using CoenM.ImageHash.HashAlgorithms;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.PresentationObjects;
using Pamaxie.MediaDetection;
using PamaxieML.Model;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using static PamaxieML.Model.OutputProperties;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class ImageComparisonViewModel : NotifyPropertyChanges
    {
        private async Task LoadImagesFromDataSourceAsyncCommandCallback(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_sourceDirectoryName))
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Error",
                    "You forgot to select or enter a folder where the images should be loaded from. We require this to load the images.");
                return;
            }

            if (!Directory.Exists(_sourceDirectoryName))
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Error",
                    "The currently selected folder doesn't exist anymore or can't be read from. Please ensure that the folder exists and can be read from.");
                return;
            }

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            ProcessingFiles = true;
            double currentProgress = 0;
            var progressDialogController = await this._dialogCoordinator.ShowProgressAsync(this, "Loading Files",
                "Please wait while we are loading data", true,
                new MetroDialogSettings()
                { CancellationToken = tokenSource.Token }
            );
            progressDialogController.Minimum = 0;
            await Task.Run(async () =>
            {
                var po = new ParallelOptions
                {
                    CancellationToken = tokenSource.Token,
                    MaxDegreeOfParallelism = System.Environment.ProcessorCount
                };

                //Creating a new list and adding the already existing images to it right away
                PoImageData[] imageQueue;
                bool working = true;

                new Thread(async () =>
                {
                    var files = System.IO.Directory.GetFiles(_sourceDirectoryName, "*", SearchOption.AllDirectories);

                    //Set the capacity of our Queue
                    imageQueue = new PoImageData[files.Length + 20];

                    //Get the amount of files in the directory, this includes files we potentially aren't interested in but better safe than sorry
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        progressDialogController.Maximum = files.Length;
                    });

                    try
                    {
                        for (var index = 0; index < files.Length; index++)
                        {
                            var file = files[index];
                            FileInfo fi = new FileInfo(file);
                            if (fi.Directory == null)
                                return;

                            var imageData = new PoImageData()
                            {
                                AssumedLabel = fi.Directory.Name,
                                FileLocation = file,
                                Name = fi.Name
                            };
                            imageQueue[index] = imageData;
                            po.CancellationToken.ThrowIfCancellationRequested();
                            currentProgress++;
                        }
                    }
                    catch (IOException ex)
                    {
                        await this._dialogCoordinator.ShowMessageAsync(this, "Error",
                            "Something went wrong while loading in the files. The error was: \n" + ex.Message);
                    }
                    catch (System.OperationCanceledException)
                    {

                    }
                    catch (AggregateException)
                    {
                        ;
                    }
                    finally
                    {
                        ProcessingFiles = false;
                        working = false;
                        await progressDialogController.CloseAsync();
                    }

                    LoadedImages = new ObservableCollection<PoImageData>(imageQueue.Where(x => x != null));

                    working = false;
                }).Start();




                while (working)
                {
                    //Update the Ui / Progress
                    await Task.Delay(200, tokenSource.Token);
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        if (progressDialogController.IsCanceled)
                            tokenSource.Cancel();
                        if (progressDialogController.IsOpen)
                            progressDialogController.SetProgress(currentProgress);
                    });
                }

                if (progressDialogController.IsOpen)
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        try
                        {
                            progressDialogController.CloseAsync();
                        }
                        catch (Exception)
                        {

                        }

                    });
            }, tokenSource.Token);

            await this._dialogCoordinator.ShowMessageAsync(this, "Success", "All files were successfully loaded");
            SourceDirectoryName = string.Empty;
            ProcessingFiles = false;
        }

        private bool CheckIfReady()
        {
            return !string.IsNullOrEmpty(_outputDirectoryName) && LoadedImages.Count > 0;
        }

        private async Task SelectDirectoryDialogAsyncCommandCallback(CancellationToken arg, bool isOutput)
        {
            var folderPicker = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            var result = folderPicker.ShowDialog();
            if (result is false or null)
                return;

            await Dispatcher.CurrentDispatcher.InvokeAsync((() =>
            {
                var selectedPath = folderPicker.SelectedPath;
                if (isOutput)
                {
                    OutputDirectoryName = selectedPath;
                    return;
                }

                SourceDirectoryName = selectedPath;
            }));
        }

        private Task CancelImagePreparationCommandCallback(CancellationToken arg)
        {
            _workToken.Cancel();
            return Task.CompletedTask;
        }

        private async Task<HashSet<string>> FilterImages(ParallelOptions po)
        {
            return await Task.Run(() =>
            {
                HashSet<ulong> existingImages = new HashSet<ulong>();
                HashSet<string> imagesToUse = new HashSet<string>();

                Mutex hashComparisonMutex = new Mutex();

                Parallel.ForEach(LoadedImages, po, (image) =>
                {
                    try
                    {
                        var hashAlgorithm = new AverageHash();
                        string filename = image.FileLocation;
                        using var stream = File.OpenRead(filename);
                        var fileType = FileDetection.DetermineFileType(stream);
                        if (fileType == null)
                            return;
                        ulong imageHash = hashAlgorithm.Hash(stream);
                        hashComparisonMutex.WaitOne();
                        imagesToUse.Add(image.FileLocation);
                        existingImages.Add(imageHash);

                        hashComparisonMutex.ReleaseMutex();
                    }
                    catch (Exception ex)
                    {
                        hashComparisonMutex.ReleaseMutex();
                    }
                });
                return imagesToUse;
            });
        }

        private static async Task SetProcessingMessage(ProgressDialogController processingFilesProgressView, string message)
        => await Application.Current.Dispatcher.InvokeAsync(() => { processingFilesProgressView.SetMessage(message); });

        private void UpdateProgressBarTimerCallback(ProgressDialogController processingFilesProgressView, ref int progress)
        {
            var currentProgress = progress;
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (processingFilesProgressView.IsCanceled)
                    _workToken.Cancel();
                processingFilesProgressView.SetProgress(currentProgress);

                if (processingTimeStopwatch != null)
                {
                    SetProcessingMessage(processingFilesProgressView, "Analyzing Files, Current speed: " + Math.Round((processingTimeStopwatch.Elapsed.TotalMilliseconds / currentProgress), 2) + "ms Per file").ConfigureAwait(false);
                }
            });
            _progressUpdateTimer.Change(300, Timeout.Infinite);
        }

        private void SetupFolderStructure(ref int progress, double maxProgress)
        {
            var predictionResults = Enum.GetValues(typeof(ImagePredictionResult)).Cast<ImagePredictionResult>().ToArray();
            var stepDistance = maxProgress / (predictionResults?.Count() * (ComparisonSettings.PercentileFolders ? 1 : 10));
            foreach (ImagePredictionResult directory in predictionResults)
            {
                string newDir = OutputDirectoryName + '\\' + directory;
                if (Directory.Exists(newDir)) continue;
                    Directory.CreateDirectory(newDir);


                if (ComparisonSettings.PercentileFolders)
                {
                    for (int i = 0; i < 10; i++)
                        Directory.CreateDirectory(newDir + "\\" + i * 10 + "%");
                    progress += (int) stepDistance;
                }
            }
        }

        private async Task StartImagePerpetrationAsyncCommandCallback(CancellationToken arg)
        {
            //Cancel if we're already processing
            if (IsBusy)
                return;

            int processingProgress = 0;
            IsBusy = true;
            ProcessingFiles = true;
            string errors = string.Empty;
            var processingFilesProgressView = await this._dialogCoordinator.ShowProgressAsync(this, "Processing Files",
                "Please wait while we are processing your files", true,
                new MetroDialogSettings()
                {
                    CancellationToken = _workToken.Token,
                    DialogResultOnCancel = MessageDialogResult.Canceled
                }
            );
            processingFilesProgressView.Minimum = 0;
            processingFilesProgressView.Maximum = LoadedImages.Count;

            ParallelOptions po = new ParallelOptions
            {
                CancellationToken = _workToken.Token,
                MaxDegreeOfParallelism = System.Environment.ProcessorCount
            };

            await Task.Run(async () =>
            {
                _progressUpdateTimer = new Timer((_ =>
                    UpdateProgressBarTimerCallback(processingFilesProgressView, ref processingProgress)));
                _progressUpdateTimer.Change(300, Timeout.Infinite);

                try
                {
                    HashSet<string> imagesToUse = null;
                    if (ComparisonSettings.HashComparison)
                    {
                        await SetProcessingMessage(processingFilesProgressView,
                            "Generating a dictionary of existing hashes and making sure they are unique");
                        imagesToUse = await FilterImages(po);
                    }

                    processingProgress = 0;

                    await SetProcessingMessage(processingFilesProgressView, "Setting up folder structure");
                    SetupFolderStructure(ref processingProgress, processingFilesProgressView.Maximum);

                    processingProgress = 0;

                    await SetProcessingMessage(processingFilesProgressView,
                        "Analyzing files");
                    processingProgress = 0;

                    processingTimeStopwatch = new Stopwatch();
                    processingTimeStopwatch.Start();
                    // Add input data
                    ModelInput input = new ModelInput();
                    foreach (var file in LoadedImages)
                    {
                        try
                        {
                            if (imagesToUse != null && !imagesToUse.Contains(file.FileLocation))
                                continue;

                            input.ImageSource = file.FileLocation;
                            // Load model and predict output of sample data
                            input.Predict(out OutputProperties labelResult);
                            string folder = labelResult.PredictedLabel.ToString();
                            if (ComparisonSettings.PercentileFolders)
                            {
                                folder = OutputDirectoryName + '\\' + folder + '\\';
                                switch (labelResult.PredictedLabel)
                                {
                                    case OutputProperties.ImagePredictionResult.Gore:
                                        folder += GetLikelyHoodFolderNumber(labelResult.GoreLikelihood) + "%";
                                        break;
                                    case OutputProperties.ImagePredictionResult.None:
                                        folder += GetLikelyHoodFolderNumber(labelResult.NoneLikelihood) + "%";
                                        break;
                                    case OutputProperties.ImagePredictionResult.Pornographic:
                                        folder += GetLikelyHoodFolderNumber(labelResult.PornographicLikelihood) + "%";
                                        break;
                                    case OutputProperties.ImagePredictionResult.Racy:
                                        folder += GetLikelyHoodFolderNumber(labelResult.RacyLikelihood) + "%";
                                        break;
                                    case OutputProperties.ImagePredictionResult.Violence:
                                        folder += GetLikelyHoodFolderNumber(labelResult.ViolenceLikelihood) + "%";
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                File.Move(file.FileLocation,  folder + '\\' + file.Name);
                            }
                            else
                            {
                                File.Move(file.FileLocation, OutputDirectoryName + '\\' + folder + '\\' + file.Name);

                            }

                            _workToken.Token.ThrowIfCancellationRequested();
                            processingProgress++;
                        }
                        catch (Exception ex)
                        {

                            if (ex is not IOException or NullReferenceException or UnknownImageFormatException)
                            {
                                throw;
                            }

                            if (ComparisonSettings.StopOnError)
                            {
                                _workToken.Cancel();
                            }

                            errors += file.Name;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is not System.OperationCanceledException or AggregateException)
                    {
                        throw;
                    }
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProcessingFiles = false;
                        processingFilesProgressView.CloseAsync();
                    });
                }

            }, _workToken.Token);

            if (processingTimeStopwatch != null)
            {
                processingTimeStopwatch.Stop();
                processingTimeStopwatch = null;
            }

            if (_progressUpdateTimer != null)
            {
                await _progressUpdateTimer.DisposeAsync();
            }

            //wait for the operation to finish.
            await processingFilesProgressView.CloseAsync();
            if (errors.Any() && !_workToken.IsCancellationRequested)
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Partial Success",
                    "Some files failed during processing. These files were: " + errors);
            }
            else if (_workToken.IsCancellationRequested)
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Aborted",
                    "The operation was cancelled by the end user.");
            }
            else
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Success",
                    "All files were successfully processed");
            }

            ProcessingFiles = false;
            IsBusy = false;
        }

        /// <summary>
        /// Get the folder where to put the items in depending on likelyhood.
        /// </summary>
        /// <param name="likelyhood"></param>
        /// <returns></returns>
        public static string GetLikelyHoodFolderNumber(float likelyhood)
        {
            likelyhood *= 100;
            var num = likelyhood - likelyhood % 10;
            if (num >= 100)
            {
                num -= 10;
            }
            return num.ToString(CultureInfo.InvariantCulture);
        }
    }
}