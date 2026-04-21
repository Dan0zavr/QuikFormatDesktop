using Microsoft.Extensions.Options;
using PDFReader;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.PdfToPng;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UglyToad.PdfPig.Graphics.Operations.MarkedContent;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace QuikFormatDesktop.ViewModels.FormatViewModels
{
    public class PreviewViewModel : ViewModelBase, IDisposable
    {
        private readonly SemaphoreSlim _processSemaphore = new SemaphoreSlim(1, 1);

        private string _currentOriginalDocxDirectory;
        private string _currentFormattedDocxDirectory;

        private string _currentOriginalDocxFile;
        private string _currentFormattedDocxFile;

        private string _currentOriginalPdfDirectory;
        private string _currentOriginalPicturesDirectory;
        private string _currentOriginalPdfFile;

        private string _currentFormattedPdfDirectory;
        private string _currentFormattedPdfFile;
        private string _currentFormattedPicturesDirectory;

        private NavigationStore _navigationStore;
        private readonly IOptions<GeneralSettings> _options;
        private readonly TemplateMapper _templateMapper;
        private readonly IDialogService _dialogService;

        private bool _isOriginal;
        private bool _isLoading;

        private Process? _currentProcess;

        public PreviewViewModel(NavigationStore navigationStore, IOptions<GeneralSettings> options, TemplateMapper templateMapper, IDialogService dialogService)
        {
            _navigationStore = navigationStore;
            _options = options;
            _templateMapper = templateMapper;
            _dialogService = dialogService;
            IsOriginal = true;
            IsLoading = true;

            OriginalPictures.CollectionChanged += Items_CollectionChanged;
        }

        public string CurrentOriginalDocxDirectory
        {
            get => _currentOriginalDocxDirectory;
            set
            {
                _currentOriginalDocxDirectory = value;
                OnPropertyChanged(nameof(CurrentOriginalDocxDirectory));
            }
        }
        public string CurrentFormattedDocxDirectory
        {
            get => _currentFormattedDocxDirectory;
            set
            {
                if (Directory.Exists(_currentFormattedDocxDirectory))
                {
                    Directory.Delete(_currentFormattedDocxDirectory, true);
                }
                _currentFormattedDocxDirectory = value;
                OnPropertyChanged(nameof(CurrentFormattedDocxDirectory));
            }
        }

        public string CurrentOriginalDocxFile
        {
            get => _currentOriginalDocxFile;
            set
            {
                _currentOriginalDocxFile = value;
                OnPropertyChanged(nameof(CurrentOriginalDocxFile));
            }
        }
        public string CurrentFormattedDocxFile
        {
            get => _currentFormattedDocxFile;
            set
            {
                _currentFormattedDocxFile = value;
                OnPropertyChanged(nameof(_currentFormattedDocxFile));
            }
        }

        public string CurrentOriginalPdfDirectory {
            get => _currentOriginalPdfDirectory;
            set
            {
                if (Directory.Exists(_currentOriginalPdfDirectory))
                {
                    Directory.Delete(_currentOriginalPdfDirectory, true);
                }
                _currentOriginalPdfDirectory = value;
                OnPropertyChanged(nameof(CurrentOriginalPdfDirectory));
            }
        }
        public string CurrentOriginalPicturesDirectory
        {
            get => _currentOriginalPicturesDirectory;
            set
            {
                if (Directory.Exists(_currentOriginalPicturesDirectory))
                {
                    Directory.Delete(_currentOriginalPicturesDirectory, true);
                }
                _currentOriginalPicturesDirectory = value;
                OnPropertyChanged(nameof(CurrentOriginalPicturesDirectory));
            }
        }
        public string CurrentOriginalPdfFile
        {
            get => _currentOriginalPdfFile;
            set
            {
                _currentOriginalPdfFile = value;
                OnPropertyChanged(nameof(CurrentOriginalPdfFile));
            }
        }

        public string CurrentFormattedPdfDirectory
        {
            get => _currentFormattedPdfDirectory;
            set
            {
                if (Directory.Exists(_currentFormattedPdfDirectory))
                {
                    Directory.Delete(_currentFormattedPdfDirectory, true);
                }
                _currentFormattedPdfDirectory = value;
                OnPropertyChanged(nameof(CurrentFormattedPdfDirectory));
            }
        }
        public string CurrentFormattedPdfFile
        {
            get => _currentFormattedPdfFile;
            set
            {
                _currentFormattedPdfFile = value;
                OnPropertyChanged(nameof(CurrentFormattedPdfFile));
            }
        }
        public string CurrentFormattedPicturesDirectory
        {
            get => _currentFormattedPicturesDirectory;
            set
            {
                if (Directory.Exists(_currentFormattedPicturesDirectory))
                {
                    Directory.Delete(_currentFormattedPicturesDirectory);
                }
                _currentFormattedPicturesDirectory= value;
                OnPropertyChanged(nameof(CurrentFormattedPicturesDirectory));
            }
        }

        public ObservableCollection<ImageItem> OriginalPictures { get; set; } = new ObservableCollection<ImageItem>();
        public ObservableCollection<ImageItem> FormattedPictures { get; set; } = new ObservableCollection<ImageItem>();
        public bool IsOriginal
        {
            get => _isOriginal;
            set
            {
                _isOriginal = value;
                OnPropertyChanged(nameof(IsOriginal));
            }
        }
        public bool IsFormattedDocumentDone { get; set; } = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public async void OnDocumentChanged()
        {
            try
            {
                await _processSemaphore.WaitAsync();

                IsLoading = true;
                IsFormattedDocumentDone = false;
                if (_navigationStore.CurrentViewModel is FormatViewModel formatViewModel)
                {
                    CurrentOriginalDocxFile = formatViewModel.SelectorCardViewModel.DocumentPath;
                }

                if (CurrentOriginalDocxFile == null) return;
                else CurrentOriginalDocxDirectory = Directory.GetParent(CurrentOriginalDocxFile).FullName;

                await ConvertOriginalDocxToPictures();
                await FormatAndConvertDocxToPictures();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при конвертации: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                _processSemaphore.Release();
            }
        }

        private async Task ConvertOriginalDocxToPictures()
        {
            CurrentOriginalPdfDirectory = CreateTempPath();

            CurrentOriginalPicturesDirectory = await ConvertToPictures(CurrentOriginalPdfFile, CurrentOriginalDocxFile, CurrentOriginalPdfDirectory, OriginalPictures);
        }

        private async Task FormatAndConvertDocxToPictures()
        {
            if (_navigationStore.CurrentViewModel is FormatViewModel formatViewModel)
            {
                if (formatViewModel.SelectorCardViewModel.Template != null)
                {
                    CurrentFormattedDocxDirectory = CreateTempPath();
                    CurrentFormattedPdfDirectory = CreateTempPath();

                    XMLParser.ParseManager parseManager = new XMLParser.ParseManager();

                    XMLParser.Styles.Template template = await _templateMapper.MapToParser(formatViewModel.SelectorCardViewModel.Template);
                    int[] ignoredPages = GetIgnoredPages(OriginalPictures);

                    CurrentFormattedDocxFile = parseManager.MainScript(CurrentOriginalDocxFile, CurrentFormattedDocxDirectory, template, ignoredPages, CurrentOriginalPdfFile);

                    CurrentFormattedPicturesDirectory = await ConvertToPictures(CurrentFormattedPdfFile, CurrentFormattedDocxFile, CurrentFormattedPdfDirectory, FormattedPictures);

                    IsFormattedDocumentDone = true;
                }
            }
        }

        private async Task<string> ConvertToPictures(string pdfFile, string docxFile, string pdfDirectory, ObservableCollection<ImageItem> picturesList)
        {
            pdfFile = await Convert(docxFile, pdfDirectory);
            string picturesDirectory = Directory.CreateDirectory(Path.Combine(pdfDirectory, "Pictures")).FullName;
            picturesList.Clear();
            var pictures = PdfToPngConverter.Convert(pdfFile, picturesDirectory);

            foreach (var p in pictures)
            {
                picturesList.Add(new ImageItem { Image = LoadImage(p), IsSelected = false });
            }
            return picturesDirectory;
        }

        private async Task<string> Convert(string docxPath, string pdfPath)
        {
            if (_currentProcess != null && !_currentProcess.HasExited)
            {
                var oldProcess = _currentProcess;

                oldProcess.Kill();

                await oldProcess.WaitForExitAsync();
                oldProcess.Dispose();
            }

            if (docxPath == null)
            {
                return null;
            }
                
            Priority priority = _options.Value.ConverterPriority;

            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PdfConverter.exe");

            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"\"{priority.ToString()}\" \"{docxPath}\" \"{pdfPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            startInfo.StandardOutputEncoding = Encoding.UTF8;
            startInfo.StandardErrorEncoding = Encoding.UTF8;

            _currentProcess = new Process { StartInfo = startInfo };
            _currentProcess.Start();

            Task<string> outputTask = _currentProcess.StandardOutput.ReadToEndAsync();
            Task<string> errorTask = _currentProcess.StandardError.ReadToEndAsync();
            Task waitForExitTask = _currentProcess.WaitForExitAsync();

            await Task.WhenAll(outputTask, errorTask, waitForExitTask);

            string result = outputTask.Result;
            string error = errorTask.Result;

            if (_currentProcess.ExitCode != 0)
            {
                _dialogService.ShowError(error);
            }

            return result.Trim();
        }

        private string CreateTempPath()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);
            return tempFolder;
        }

        public async void OnTemplateChanged()
        {
            try
            {
                await _processSemaphore.WaitAsync();
                IsLoading = true;

                IsFormattedDocumentDone = false;

                if (_navigationStore.CurrentViewModel is FormatViewModel formatViewModel)
                {
                    if (formatViewModel.SelectorCardViewModel.DocumentPath != null)
                    {
                        await FormatAndConvertDocxToPictures();
                    }
                }
            }
            finally
            {
                IsLoading = false;
                _processSemaphore.Release();
            }
        }

        private BitmapImage LoadImage(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            return bitmap;
        }

        public int[] GetIgnoredPages(ObservableCollection<ImageItem> pages)
        {
            List<int> ignoredPagesList = new List<int>();
            for (int i = 0; i < pages.Count; i++)
            {
                if (pages[i].IsSelected)
                {
                    ignoredPagesList.Add(i+1);
                }
            }
            if (ignoredPagesList.Count > 0) 
            {
                int[] result = ignoredPagesList.ToArray();
                return result;
            }
            return null;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ImageItem item in e.NewItems)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (ImageItem item in e.OldItems)
                {
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }
        }

        private CancellationTokenSource _selectionCts;
        private async void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ImageItem.IsSelected))
                return;

            _selectionCts?.Cancel();
            _selectionCts = new CancellationTokenSource();

            try
            {
                await Task.Delay(300, _selectionCts.Token);

                await HandleSelectionChangedAsync(_selectionCts.Token);
            }
            catch (TaskCanceledException)
            {
            }
        }

        private async Task HandleSelectionChangedAsync(CancellationToken token)
        {
            try
            {
                await _processSemaphore.WaitAsync();
                var selectedItems = OriginalPictures
                    .Where(i => i.IsSelected)
                    .ToList();

                await Task.Run(async () =>
                {
                    token.ThrowIfCancellationRequested();

                    await FormatAndConvertDocxToPictures();

                }, token);
            }
            finally
            {
                _processSemaphore.Release();
            }
        }

        public void Dispose()
        {
            try
            {
                _currentProcess?.Kill();
                _currentProcess?.Dispose();

                if (Directory.Exists(CurrentFormattedPdfDirectory)) Directory.Delete(CurrentFormattedPdfDirectory, true);
                if (Directory.Exists(CurrentFormattedDocxDirectory)) Directory.Delete(CurrentFormattedDocxDirectory, true);

                if (Directory.Exists(CurrentOriginalPdfDirectory)) Directory.Delete(CurrentOriginalPdfDirectory, true);
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Произошла ошибка при удалении временных папок, пожалуйста удалите следующие папки: {CurrentFormattedDocxDirectory}, {CurrentFormattedPdfDirectory}, {CurrentOriginalDocxDirectory}");
            }
        }
    }
}
