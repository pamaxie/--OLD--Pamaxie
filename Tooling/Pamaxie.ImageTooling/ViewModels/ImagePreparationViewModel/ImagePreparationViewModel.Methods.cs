using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using Ookii.Dialogs.Wpf;
using Pamaxie.ImageTooling.PresentationObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <inheritdoc cref="ImagePreparationViewModel"/>
    public partial class ImagePreparationViewModel
    {
        private async Task LoadImagesFromDataSourceAsyncCommandCallback(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_sourceFolderName))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error",
                    "You forgot to select or enter a folder where the images should be loaded from. We require this to load the images.");
                return;
            }

            if (!Directory.Exists(_sourceFolderName))
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error",
                    "The currently selected folder doesn't exist anymore or can't be read from. Please ensure that the folder exists and can be read from.");
                return;
            }

            CancellationTokenSource tokenSource = new();
            LoadingFiles = true;
            double currentProgress = 0;
            ProgressDialogController progressDialogController = await _dialogCoordinator.ShowProgressAsync(this,
                "Loading Files",
                "Please wait while we are loading data", true,
                new MetroDialogSettings {CancellationToken = tokenSource.Token}
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
                    string[] files = Directory.GetFiles(_sourceFolderName, "*", SearchOption.AllDirectories);

                    //Set the capacity of our Queue
                    imageQueue = new PoImageData[files.Length + 20];

                    //Get the amount of files in the directory, this includes files we potentially aren't interested in but better safe than sorry
                    Application.Current.Dispatcher.Invoke(() => { progressDialogController.Maximum = files.Length; });

                    try
                    {
                        for (int index = 0; index < files.Length; index++)
                        {
                            string file = files[index];
                            FileInfo fi = new(file);
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
                        LoadingFiles = false;
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
            SourceFolderName = string.Empty;
            LoadingFiles = false;
        }

        private bool CheckIfReady()
        {
            return !string.IsNullOrEmpty(_destinationFolderName) && LoadedImages.Count > 0;
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
                    DestinationFolderName = selectedPath;
                    return;
                }

                SourceFolderName = selectedPath;
            });
        }

        private Task CancelImagePreparationCommandCallback(CancellationToken arg)
        {
            _workToken.Cancel();
            return Task.CompletedTask;
        }

        private void UpdateProgressBarTimerCallback(ProgressDialogController processingFilesProgressView, int progress)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (processingFilesProgressView.IsCanceled)
                    _workToken.Cancel();
                processingFilesProgressView.SetProgress(progress);
            });
            _progressUpdateTimer.Change(200, Timeout.Infinite);
        }

        private async Task StartImagePerpetrationAsyncCommandCallback(CancellationToken arg)
        {
            //Cancel if we're already processing
            if (IsBusy)
                return;

            int processingProgress = 0;
            IsBusy = true;
            LoadingFiles = true;
            string errors = string.Empty;
            if (int.TryParse(ProcessingSettings.Width, out int width))
                width = 0;
            if (int.TryParse(ProcessingSettings.Height, out int height))
                height = 0;
            ProgressDialogController processingFilesProgressView = await _dialogCoordinator.ShowProgressAsync(this,
                "Processing Files",
                "Please wait while we are processing your files", true,
                new MetroDialogSettings
                {
                    CancellationToken = _workToken.Token,
                    DialogResultOnCancel = MessageDialogResult.Canceled
                }
            );

            processingFilesProgressView.Minimum = 0;
            processingFilesProgressView.Maximum = LoadedImages.Count;

            await Task.Run(() =>
            {
                ParallelOptions po = new()
                {
                    CancellationToken = _workToken.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };

                _progressUpdateTimer = new Timer((_ =>
                    UpdateProgressBarTimerCallback(processingFilesProgressView, processingProgress)));
                _progressUpdateTimer.Change(200, Timeout.Infinite);

                try
                {
                    Parallel.ForEach(LoadedImages, po, (file) =>
                    {
                        try
                        {
                            string assumedDestinationDirectory = DestinationFolderName + "\\" + file.AssumedLabel;
                            if (!Directory.Exists(assumedDestinationDirectory))
                                Directory.CreateDirectory(assumedDestinationDirectory);

                            string output = assumedDestinationDirectory + "\\" + Guid.NewGuid().ToString();
                            using Image img = Image.Load(file.FileLocation);

                            var ratio = (float) img.Height / img.Width;
                            switch (width)
                            {
                                case > 0 when height > 0:
                                    img.Mutate(x => x
                                        .Resize(width, height));
                                    img.Save(output + ".jpeg", new JpegEncoder());
                                    break;
                                case > 0:
                                    img.Mutate(x => x
                                        .Resize(width, (int) (ratio * width)));
                                    img.Save(output + ".jpeg", new JpegEncoder());
                                    break;
                                default:
                                {
                                    if (height > 0)
                                    {
                                        img.Mutate(x => x
                                            .Resize((int) (ratio * height), height));
                                        img.Save(output + ".jpeg", new JpegEncoder());
                                    }

                                    break;
                                }
                            }

                            if (ProcessingSettings.MirrorImages)
                            {
                                img.Mutate(x => x
                                    .Flip(FlipMode.Horizontal));
                                img.Save(output + "2" + ".jpeg", new JpegEncoder());

                                img.Mutate(x => x
                                    .Flip(FlipMode.Vertical));
                                img.Save(output + "3" + ".jpeg", new JpegEncoder());
                            }

                            if (ProcessingSettings.ColorChange)
                            {
                                img.Mutate(x => x
                                    .ColorBlindness(ColorBlindnessMode.Achromatomaly));
                                img.Save(output + "4" + ".jpeg", new JpegEncoder());
                            }

                            processingProgress++;

                            img.Dispose();
                            po.CancellationToken.ThrowIfCancellationRequested();
                        }
                        catch (Exception ex)
                        {
                            if (ex is not IOException && ex is not NullReferenceException &&
                                ex is not UnknownImageFormatException) throw;
                            if (ProcessingSettings.StopOnError)
                            {
                                _workToken.Cancel();
                            }

                            errors += file.Name + '\n';
                        }
                    });
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
                        LoadingFiles = false;
                        processingFilesProgressView.CloseAsync();
                    });
                }
            }, _workToken.Token);

            if (_progressUpdateTimer != null)
            {
                await _progressUpdateTimer.DisposeAsync();
            }

            //wait for the operation to finish.
            await processingFilesProgressView.CloseAsync();
            if (errors.Any() && !_workToken.IsCancellationRequested)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Partial Success",
                    "Some files failed during processing. These files were: \n" + errors);
            }
            else if (_workToken.IsCancellationRequested)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Aborted",
                    "The operation was cancelled by the end user.");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Success", "All files were successfully processed");
            }

            LoadingFiles = false;
            IsBusy = false;
        }
    }
}