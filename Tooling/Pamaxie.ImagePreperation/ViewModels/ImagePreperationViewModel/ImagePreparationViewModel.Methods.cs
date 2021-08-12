using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.PresentationObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class ImagePreparationViewModel : NotifyPropertyChanges
    {
        private async Task LoadImagesFromDataSourceAsyncCommandCallback(CancellationToken cancellationToken)
        {



            if (string.IsNullOrEmpty(_sourceFolderName))
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Error",
                    "You forgot to select or enter a folder where the images should be loaded from. We require this to load the images.");
                return;
            }

            if (!Directory.Exists(_sourceFolderName))
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Error",
                    "The currently selected folder doesn't exist anymore or can't be read from. Please ensure that the folder exists and can be read from.");
                return;
            }

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            LoadingFiles = true;
            double currentProgress = 0;
            var progressDialogController = await this._dialogCoordinator.ShowProgressAsync(this, "Loading Files",
                "Please wait while we are loading data", true,
                new MetroDialogSettings()
                    { CancellationToken = tokenSource.Token }
            );
            progressDialogController.Minimum = 0;
            await Task.Run(async () =>
            {
                ParallelOptions po = new ParallelOptions
                {
                    CancellationToken = tokenSource.Token, 
                    MaxDegreeOfParallelism = System.Environment.ProcessorCount
                };

                //Creating a new list and adding the already existing images to it right away
                PoImageData[] imageQueue;
                bool working = true;

                new Thread(async () =>
                {
                    var files = System.IO.Directory.GetFiles(_sourceFolderName, "*", SearchOption.AllDirectories);

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
                    await progressDialogController.CloseAsync();
            }, tokenSource.Token);

            await this._dialogCoordinator.ShowMessageAsync(this, "Success", "All files were successfully loaded");
            SourceFolderName = string.Empty;
            LoadingFiles = false;
        }

        private bool CheckIfReady()
        {
            return !string.IsNullOrEmpty(_destinationFolderName) && LoadedImages.Count > 0;
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
                    DestinationFolderName = selectedPath;
                    return;
                }
                SourceFolderName = selectedPath;
            }));
        }

        private Task CancelImagePreparationCommandCallback(CancellationToken arg)
        {
            _workToken.Cancel();
            return Task.CompletedTask;
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
            int.TryParse(ProcessingSettings.Width, out var width);
            int.TryParse(ProcessingSettings.Height, out var height);
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

            //TODO: I see optimization potential here, sadly don't have any idea how to implement this and keep the progress visible. Help is wanted.
            await Task.Run(async () =>
            {
                bool working = true;
                ParallelOptions po = new ParallelOptions
                {
                    CancellationToken = _workToken.Token, 
                    MaxDegreeOfParallelism = System.Environment.ProcessorCount
                };

                new Thread(() =>
                {
                    try
                    {
                        Parallel.ForEach(LoadedImages, po, (file) =>
                        {
                            try
                            {
                                var assumedDestinationDirectory = DestinationFolderName + "\\" + file.AssumedLabel;
                                if (!Directory.Exists(assumedDestinationDirectory))
                                    Directory.CreateDirectory(assumedDestinationDirectory);

                                var output = assumedDestinationDirectory + "\\" + Guid.NewGuid().ToString();
                                using var img = Image.Load(file.FileLocation);

                                var ratio = (float) img.Height / (float) img.Width;
                                if (width > 0 && height > 0)
                                {
                                    img.Mutate(x => x
                                        .Resize(width, height));
                                    img.Save(output + ".jpeg", new JpegEncoder());
                                }
                                else if (width > 0)
                                {
                                    img.Mutate(x => x
                                        .Resize(width, (int) (ratio * width)));
                                    img.Save(output + ".jpeg", new JpegEncoder());
                                }
                                else if (height > 0)
                                {
                                    img.Mutate(x => x
                                        .Resize((int) (ratio * height), height));
                                    img.Save(output + ".jpeg", new JpegEncoder());
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
                            catch (IOException)
                            {
                                if (ProcessingSettings.StopOnError)
                                {
                                    _workToken.Cancel();
                                }

                                errors += file.Name;
                            }
                            catch (NullReferenceException)
                            {
                                if (ProcessingSettings.StopOnError)
                                    _workToken.Cancel();

                                errors += file.Name;
                            }
                            catch (UnknownImageFormatException)
                            {
                                if (ProcessingSettings.StopOnError)
                                    _workToken.Cancel();

                                errors += file.Name;
                            }
                        });
                    }
                    catch (System.OperationCanceledException)
                    {

                    }
                    catch (AggregateException)
                    {

                    }
                    finally
                    {
                        LoadingFiles = false;
                        working = false;
                        processingFilesProgressView.CloseAsync();
                    }

                    working = false;
                }).Start();
                while (working)
                {
                    //Update the Ui / Progress
                    await Task.Delay(200, arg);
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        if (processingFilesProgressView.IsCanceled)
                            _workToken.Cancel();
                        processingFilesProgressView.SetProgress(processingProgress);
                    });
                }
            }, _workToken.Token);

            //wait for the operation to finish.
            await processingFilesProgressView.CloseAsync();
            if (errors.Any() && !_workToken.IsCancellationRequested)
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Partial Success",
                    "Some files failed during processing. These files were: " + errors);
            }else if (_workToken.IsCancellationRequested)
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Aborted", "The operation was cancelled by the end user.");
            }
            else
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Success", "All files were successfully processed");
            }

            LoadingFiles = false;
            IsBusy = false;
        }
    }
}
