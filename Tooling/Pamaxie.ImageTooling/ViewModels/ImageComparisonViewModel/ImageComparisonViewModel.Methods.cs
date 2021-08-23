using System;
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
using MahApps.Metro.Controls.Dialogs;
using Ookii.Dialogs.Wpf;
using Pamaxie.ImageTooling.PresentationObjects;
using Pamaxie.MediaDetection;
using PamaxieML.Model;
using SixLabors.ImageSharp;
using static PamaxieML.Model.OutputProperties;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <inheritdoc cref="ImageComparisonViewModel"/>
    public partial class ImageComparisonViewModel
    {
        private async Task LoadImagesFromDataSourceAsyncCommandCallback(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_sourceDirectoryName))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error",
                    "You forgot to select or enter a folder where the images should be loaded from. We require this to load the images.");
                return;
            }

            if (!Directory.Exists(_sourceDirectoryName))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error",
                    "The currently selected folder doesn't exist anymore or can't be read from. Please ensure that the folder exists and can be read from.");
                return;
            }

            CancellationTokenSource tokenSource = new();
            ProcessingFiles = true;
            double currentProgress = 0;
            ProgressDialogController progressDialogController = await this._dialogCoordinator.ShowProgressAsync(this,
                "Loading Files",
                "Please wait while we are loading data", true,
                new MetroDialogSettings()
                    {CancellationToken = tokenSource.Token}
            );
            progressDialogController.Minimum = 0;
            await Task.Run(async () =>
            {
                ParallelOptions po = new()
                {
                    CancellationToken = tokenSource.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };

                //Creating a new list and adding the already existing images to it right away
                PoImageData[] imageQueue;
                bool working = true;

                new Thread(async () =>
                {
                    string[] files = Directory.GetFiles(_sourceDirectoryName, "*", SearchOption.AllDirectories);

                    //Set the capacity of our Queue
                    imageQueue = new PoImageData[files.Length + 20];

                    //Get the amount of files in the directory, this includes files we potentially aren't interested in but better safe than sorry
                    Application.Current.Dispatcher.Invoke(() => { progressDialogController.Maximum = files.Length; });

                    try
                    {
                        for (int index = 0; index < files.Length; index++)
                        {
                            string file = files[index];
                            FileInfo fi = new FileInfo(file);
                            if (fi.Directory == null)
                                return;

                            PoImageData imageData = new()
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
                        await _dialogCoordinator.ShowMessageAsync(this, "Error",
                            "Something went wrong while loading in the files. The error was: \n" + ex.Message);
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch (AggregateException)
                    {
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
                        catch
                        {
                            // ignored
                        }
                    });
            }, tokenSource.Token);

            await _dialogCoordinator.ShowMessageAsync(this, "Success", "All files were successfully loaded");
            SourceDirectoryName = string.Empty;
            ProcessingFiles = false;
        }

        private bool CheckIfReady()
        {
            return !string.IsNullOrEmpty(_outputDirectoryName) && LoadedImages.Count > 0;
        }

        // ReSharper disable once UnusedParameter.Local
        private async Task SelectDirectoryDialogAsyncCommandCallback(bool isOutput, CancellationToken arg)
        {
            VistaFolderBrowserDialog folderPicker = new();
            bool? result = folderPicker.ShowDialog();
            if (result is false or null)
                return;

            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                string selectedPath = folderPicker.SelectedPath;
                if (isOutput)
                {
                    OutputDirectoryName = selectedPath;
                    return;
                }

                SourceDirectoryName = selectedPath;
            });
        }

        private Task CancelImageComparisonCommandCallback(CancellationToken arg)
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

                Mutex hashComparisonMutex = new();

                Parallel.ForEach(LoadedImages, po, (image) =>
                {
                    try
                    {
                        AverageHash hashAlgorithm = new();
                        string filename = image.FileLocation;
                        using FileStream stream = File.OpenRead(filename);
                        KeyValuePair<FileSpecification, FileType>? fileType = stream.DetermineFileType();
                        if (fileType == null)
                            return;
                        ulong imageHash = hashAlgorithm.Hash(stream);
                        hashComparisonMutex.WaitOne();
                        imagesToUse.Add(image.FileLocation);
                        existingImages.Add(imageHash);

                        hashComparisonMutex.ReleaseMutex();
                    }
                    catch
                    {
                        hashComparisonMutex.ReleaseMutex();
                    }
                });
                return imagesToUse;
            });
        }

        private static async Task SetProcessingMessage(ProgressDialogController processingFilesProgressView,
            string message)
            => await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                processingFilesProgressView.SetMessage(message);
            });

        private void UpdateProgressBarTimerCallback(ProgressDialogController processingFilesProgressView,
            ref int progress)
        {
            var currentProgress = progress;
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (processingFilesProgressView.IsCanceled)
                    _workToken.Cancel();
                processingFilesProgressView.SetProgress(currentProgress);

                if (_processingTimeStopwatch != null)
                {
                    SetProcessingMessage(processingFilesProgressView,
                        "Analyzing Files, Current speed: " +
                        Math.Round((_processingTimeStopwatch.Elapsed.TotalMilliseconds / currentProgress), 2) +
                        "ms Per file").ConfigureAwait(false);
                }
            });
            _progressUpdateTimer.Change(300, Timeout.Infinite);
        }

        private void SetupFolderStructure(ref int progress, double maxProgress)
        {
            ImagePredictionResult[] predictionResults =
                Enum.GetValues(typeof(ImagePredictionResult)).Cast<ImagePredictionResult>().ToArray();
            double? stepDistance =
                maxProgress / (predictionResults.Length * (ComparisonSettings.PercentileFolders ? 1 : 10));
            foreach (ImagePredictionResult directory in predictionResults)
            {
                string newDir = OutputDirectoryName + '\\' + directory;
                if (Directory.Exists(newDir)) continue;
                Directory.CreateDirectory(newDir);

                if (!ComparisonSettings.PercentileFolders) continue;
                for (int i = 0; i < 10; i++)
                    Directory.CreateDirectory(newDir + "\\" + i * 10 + "%");
                progress += (int) stepDistance;
            }
        }

        private async Task StartImageComparisonAsyncCommandCallback(CancellationToken arg)
        {
            //Cancel if we're already processing
            if (IsBusy)
                return;

            int processingProgress = 0;
            IsBusy = true;
            ProcessingFiles = true;
            string errors = string.Empty;
            ProgressDialogController processingFilesProgressView = await _dialogCoordinator.ShowProgressAsync(this,
                "Processing Files",
                "Please wait while we are processing your files", true,
                new MetroDialogSettings()
                {
                    CancellationToken = _workToken.Token,
                    DialogResultOnCancel = MessageDialogResult.Canceled
                }
            );
            processingFilesProgressView.Minimum = 0;
            processingFilesProgressView.Maximum = LoadedImages.Count;

            ParallelOptions po = new()
            {
                CancellationToken = _workToken.Token,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            await Task.Run(async () =>
            {
                _progressUpdateTimer = new Timer((_ =>
                    // ReSharper disable once AccessToModifiedClosure
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

                    _processingTimeStopwatch = new Stopwatch();
                    _processingTimeStopwatch.Start();
                    // Add input data
                    ModelInput input = new ModelInput();
                    foreach (PoImageData file in LoadedImages)
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
                                folder += labelResult.PredictedLabel switch
                                {
                                    ImagePredictionResult.Gore => GetLikelyHoodFolderNumber(labelResult
                                        .GoreLikelihood) + "%",
                                    ImagePredictionResult.None => GetLikelyHoodFolderNumber(labelResult
                                        .NoneLikelihood) + "%",
                                    ImagePredictionResult.Pornographic => GetLikelyHoodFolderNumber(
                                        labelResult.PornographicLikelihood) + "%",
                                    ImagePredictionResult.Racy => GetLikelyHoodFolderNumber(labelResult
                                        .RacyLikelihood) + "%",
                                    ImagePredictionResult.Violence => GetLikelyHoodFolderNumber(
                                        labelResult.ViolenceLikelihood) + "%",
                                    _ => throw new ArgumentOutOfRangeException()
                                };
                                File.Move(file.FileLocation, folder + '\\' + file.Name);
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
                    if (ex is not OperationCanceledException or AggregateException)
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

            if (_processingTimeStopwatch != null)
            {
                _processingTimeStopwatch.Stop();
                _processingTimeStopwatch = null;
            }

            if (_progressUpdateTimer != null)
            {
                await _progressUpdateTimer.DisposeAsync();
            }

            //wait for the operation to finish.
            await processingFilesProgressView.CloseAsync();
            if (errors.Any() && !_workToken.IsCancellationRequested)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Partial Success",
                    "Some files failed during processing. These files were: " + errors);
            }
            else if (_workToken.IsCancellationRequested)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Aborted",
                    "The operation was cancelled by the end user.");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Success",
                    "All files were successfully processed");
            }

            ProcessingFiles = false;
            IsBusy = false;
        }

        /// <summary>
        /// Get the folder where to put the items in depending on likelihood.
        /// </summary>
        /// <param name="likelihood"></param>
        /// <returns></returns>
        private static string GetLikelyHoodFolderNumber(float likelihood)
        {
            likelihood *= 100;
            float num = likelihood - likelihood % 10;
            if (num >= 100)
            {
                num -= 10;
            }

            return num.ToString(CultureInfo.InvariantCulture);
        }
    }
}