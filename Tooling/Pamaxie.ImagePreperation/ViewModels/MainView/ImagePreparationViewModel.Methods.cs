using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Accessibility;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using Pamaxie.ImagePreparation.PresentationObjects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Pamaxie.ImagePreparation.ViewModels
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


            LoadingFiles = true;
            double currentProgress = 0;
            DispatcherOperation addingFilesOperation = null;
            var progress = await this._dialogCoordinator.ShowProgressAsync(this, "Loading Files",
                "Please wait while we are loading data", true,
                new MetroDialogSettings()
                    { CancellationToken = cancellationToken }
            );
            progress.Minimum = 0;
            await Task.Run(() =>
            {
                try
                {
                    var subDirs = Directory.GetDirectories(_sourceFolderName);
                    foreach (var dir in subDirs)
                    {

                        var files = new DirectoryInfo(dir).GetFiles();
                        foreach (var file in files)
                        {
                            progress.Maximum++;
                            addingFilesOperation = Application.Current.Dispatcher.BeginInvoke(() =>
                            {
                                LoadedImages.Add(new PoImageData()
                                {
                                    AssumedLabel = dir.Split("\\").LastOrDefault(), 
                                    FileLocation = file.FullName,
                                    FileExtension = "." + file.Extension,
                                    Name = file.Name
                                });
                                currentProgress++;
                                progress.SetProgress(currentProgress);
                            }, DispatcherPriority.Background);
                        }
                    }
                }
                catch (IOException ex)
                {
                    this._dialogCoordinator.ShowMessageAsync(this, "Error",
                        "Something went wrong while loading in the files. The error was: \n" + ex.Message).ConfigureAwait(false);
                }

            }, cancellationToken);

            //wait for the operation to finish.
            if (addingFilesOperation != null)
            {
                await addingFilesOperation;
                await progress.CloseAsync();
            }

            await this._dialogCoordinator.ShowMessageAsync(this, "Success", "All files were successfully loaded");
            SourceFolderName = string.Empty;
            LoadingFiles = false;
        }

        private bool CheckIfReady()
        {
            return !string.IsNullOrEmpty(_destinationFolderName) && LoadedImages.Count > 0;
        }

        private void OpenHelpPageCommandCallback(object obj)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://wiki.pamaxie.com/en/tooling/image-preperation",
                UseShellExecute = true
            });
        }

        private async Task SelectDirectoryDialogAsyncCommandCallback(CancellationToken arg, bool IsOutput)
        {
            var folderPicker = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            var result = folderPicker.ShowDialog();
            if (result is false or null)
                return;

            await Dispatcher.CurrentDispatcher.InvokeAsync((() =>
            {
                var selectedPath = folderPicker.SelectedPath;
                if (IsOutput)
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
                    { CancellationToken = _workToken.Token }
            );
            processingFilesProgressView.Minimum = 0;
            processingFilesProgressView.Maximum = LoadedImages.Count;

            await Task.Run(() =>
            {
                Parallel.ForEach(LoadedImages, (file) =>
                {
                    try
                    {
                        var assumedDestinationDirectory = DestinationFolderName + "\\" + file.AssumedLabel;
                        if (!Directory.Exists(assumedDestinationDirectory))
                            Directory.CreateDirectory(assumedDestinationDirectory);

                        var output = assumedDestinationDirectory + "\\" + Guid.NewGuid().ToString();

                        using var img = Image.Load(file.FileLocation);

                        var ratio = (float)img.Height / (float)img.Width;
                        if (width > 0 && height > 0)
                        {
                            img.Mutate(x => x
                                .Resize(width, height));
                            img.Save(output, new JpegEncoder());
                        }
                        else if (width > 0)
                        {
                            img.Mutate(x => x
                                .Resize(width, (int)(ratio * width)));
                            img.Save(output, new JpegEncoder());
                        }
                        else if (height > 0)
                        {
                            img.Mutate(x => x
                                .Resize((int)(ratio * height), height));
                            img.Save(output, new JpegEncoder());
                        }

                        if (ProcessingSettings.MirrorImages)
                        {
                            img.Mutate(x => x
                                .Flip(FlipMode.Horizontal));
                            img.Save(output + "2" + file.Name + file.FileExtension, new JpegEncoder());

                            img.Mutate(x => x
                                .Flip(FlipMode.Vertical));
                            img.Save(output + "3" + file.Name + file.FileExtension, new JpegEncoder());
                        }

                        if (ProcessingSettings.ColorChange)
                        {
                            img.Mutate(x => x
                                .ColorBlindness(ColorBlindnessMode.Achromatomaly));
                            img.Save(output + "4" + file.Name + file.FileExtension, new JpegEncoder());
                        }

                        processingProgress++;

                        Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            processingFilesProgressView.SetProgress(processingProgress);
                        });
                    }
                    catch(Exception ex)
                    {
                        if (ProcessingSettings.StopOnError)
                        {
                            _workToken.Cancel();
                        }

                        errors += file.Name;
                    }
                });

                
            }, _workToken.Token);

            //wait for the operation to finish.
            await processingFilesProgressView.CloseAsync();
            if (errors.Any())
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Partial Success",
                    "Some files failed during processing. These files were: " + errors);
            }
            else
            {
                await this._dialogCoordinator.ShowMessageAsync(this, "Success", "All files were sucessfully processed");
            }

            LoadingFiles = false;
            IsBusy = false;
        }
    }
}
