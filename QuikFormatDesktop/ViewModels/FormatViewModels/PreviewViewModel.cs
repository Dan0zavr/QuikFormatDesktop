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
    public class PreviewViewModel : ViewModelBase, IAsyncDisposable
    {
        private const int WAIT_TIME = 2600;

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

        private List<string> _originalPicturePaths;
        private List<string> _formattedPicturePaths;

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

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (_originalPicturePaths != null)
                    {
                        OriginalPictures.Clear();
                        foreach (var path in _originalPicturePaths)
                            OriginalPictures.Add(new ImageItem { Image = LoadImage(path), IsSelected = false });
                    }

                    if (_formattedPicturePaths != null)
                    {
                        FormattedPictures.Clear();
                        foreach (var path in _formattedPicturePaths)
                            FormattedPictures.Add(new ImageItem { Image = LoadImage(path), IsSelected = false });
                    }
                });
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

            var (picturesDir, picturePaths) = await ConvertToPictures(CurrentOriginalPdfFile, CurrentOriginalDocxFile, CurrentOriginalPdfDirectory);
            CurrentOriginalPicturesDirectory = picturesDir;
            _originalPicturePaths = picturePaths;
        }

        private async Task FormatAndConvertDocxToPictures()
        {
            if (_navigationStore.CurrentViewModel is FormatViewModel formatViewModel)
            {
                try
                {
                    if (formatViewModel.SelectorCardViewModel.Template != null)
                    {
                        CurrentFormattedDocxDirectory = CreateTempPath();
                        CurrentFormattedPdfDirectory = CreateTempPath();

                        XMLParser.ParseManager parseManager = new XMLParser.ParseManager();

                        XMLParser.Styles.Template template = await _templateMapper.MapToParser(formatViewModel.SelectorCardViewModel.Template);
                        int[] ignoredPages = GetIgnoredPages(OriginalPictures);

                        CurrentFormattedDocxFile = parseManager.MainScript(CurrentOriginalDocxFile, CurrentFormattedDocxDirectory, template, ignoredPages, CurrentOriginalPdfFile);

                        var (picturesDir, picturePaths) = await ConvertToPictures(CurrentFormattedPdfFile, CurrentFormattedDocxFile, CurrentFormattedPdfDirectory);
                        CurrentFormattedPicturesDirectory = picturesDir;
                        _formattedPicturePaths = picturePaths;
                        IsFormattedDocumentDone = true;
                    }
                }
                catch (Exception ex)
                {
                    _dialogService.ShowError($"Произошла ошибка конвертации: {ex.Message}");
                    formatViewModel.SelectorCardViewModel.DocumentPath = null;
                }
            }
        }

        private async Task<(string picturesDirectory, List<string> pictureFiles)> ConvertToPictures(string pdfFile, string docxFile, string pdfDirectory)
        {
            pdfFile = await Convert(docxFile, pdfDirectory);
            if (string.IsNullOrEmpty(pdfFile))
                return (null, new List<string>());

            string picturesDirectory = Directory.CreateDirectory(
                Path.Combine(pdfDirectory, "Pictures")).FullName;

            var pictureFiles = PdfToPngConverter.Convert(pdfFile, picturesDirectory).ToList();

            return (picturesDirectory, pictureFiles);
        }

        private async Task<string> Convert(string docxPath, string pdfPath)
        {
            try
            {
                if (_currentProcess != null)
                {
                    try
                    {
                        if (!_currentProcess.HasExited)
                        {
                            _currentProcess.Kill();
                            await _currentProcess.WaitForExitAsync();
                        }
                    }
                    catch { }
                    finally
                    {
                        _currentProcess.Dispose();
                        _currentProcess = null;
                    }
                }

                if (docxPath == null)
                {
                    return null;
                }

                Priority priority = _options.Value.ConverterPriority;

                string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PdfConverter.exe");

                if (!Path.Exists(exePath))
                {
                    _dialogService.ShowError("Не найден файл PdfConverter.exe, переустановите приложение или добавьте файл в корневую папку");
                    return "";
                }

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

                using var process = new Process { StartInfo = startInfo };
                _currentProcess = process;
                process.Start();

                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                string result = await outputTask;
                string error = await errorTask;

                int exitCode = process.ExitCode;

                process.Close();
                _currentProcess = null;

                if (exitCode != 0)
                {
                    _dialogService.ShowError(error);
                    return string.Empty;
                }

                return result?.Trim() ?? string.Empty;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Произошла ошибка {ex}");
                return "";
            }
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
                await Task.Delay(WAIT_TIME, _selectionCts.Token);

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

                IsLoading = true;

                await Task.Run(async () =>
                {
                    token.ThrowIfCancellationRequested();

                    await FormatAndConvertDocxToPictures();

                }, token);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (_formattedPicturePaths != null)
                    {
                        FormattedPictures.Clear();
                        foreach (var path in _formattedPicturePaths)
                            FormattedPictures.Add(new ImageItem
                            {
                                Image = LoadImage(path),
                                IsSelected = false
                            });
                    }
                });
            }
            finally
            {
                IsLoading = false;
                _processSemaphore.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_currentProcess != null)
                {
                    try
                    {
                        if (!_currentProcess.HasExited)
                        {
                            _currentProcess.Kill();
                            await _currentProcess.WaitForExitAsync();
                            await Task.Delay(200);
                        }
                    }
                    catch (InvalidOperationException)
                    {

                    }
                    finally
                    {
                        _currentProcess.Dispose();
                        _currentProcess = null;
                    }
                }

                if (Directory.Exists(CurrentFormattedPdfDirectory)) Directory.Delete(CurrentFormattedPdfDirectory, true);
                if (Directory.Exists(CurrentFormattedDocxDirectory)) Directory.Delete(CurrentFormattedDocxDirectory, true);
                if (Directory.Exists(CurrentOriginalPdfDirectory)) Directory.Delete(CurrentOriginalPdfDirectory, true);

                await DeleteDirectoryWithRetry(CurrentFormattedPdfDirectory);
                await DeleteDirectoryWithRetry(CurrentFormattedDocxDirectory);
                await DeleteDirectoryWithRetry(CurrentOriginalPdfDirectory);
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Произошла ошибка при удалении временных папок, пожалуйста удалите следующие папки: {CurrentFormattedDocxDirectory}, {CurrentFormattedPdfDirectory}");
            }
        }

        private async Task DeleteDirectoryWithRetry(string path)
        {
            if (!Directory.Exists(path))
                return;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Directory.Delete(path, true);
                    return;
                }
                catch (IOException)
                {
                    await Task.Delay(200);
                }
                catch (UnauthorizedAccessException)
                {
                    await Task.Delay(200);
                }
            }

            throw new Exception($"Не удалось удалить папку: {path}");
        }
    }
}
