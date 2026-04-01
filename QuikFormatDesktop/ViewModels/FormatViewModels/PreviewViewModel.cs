using Microsoft.Extensions.Options;
using PDFReader;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.PdfToPng;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using UglyToad.PdfPig.Graphics.Operations.MarkedContent;

namespace QuikFormatDesktop.ViewModels.FormatViewModels
{
    public class PreviewViewModel : ViewModelBase
    {
        private string _currentDocxDirectory;
        private string _currentPdfDirectory;
        private string _currentPicturesDirectory;
        private string _currentPdfFile;

        private NavigationStore _navigationStore;
        private readonly IOptions<GeneralSettings> _options;

        private string _pdfPath;
        private Process? _currentProcess;

        public PreviewViewModel(NavigationStore navigationStore, IOptions<GeneralSettings> options)
        {
            _navigationStore = navigationStore;
            _options = options;
        }

        public string CurrentDocxDirectory
        {
            get => _currentDocxDirectory;
            set
            {
                if (Directory.Exists(_currentDocxDirectory))
                {
                    Directory.Delete(_currentDocxDirectory, true);
                }
                _currentDocxDirectory = value;
                OnPropertyChanged(nameof(CurrentDocxDirectory));
            }
        }
        public string CurrentPdfDirectory {
            get => _currentPdfDirectory;
            set
            {
                if (Directory.Exists(_currentPdfDirectory))
                {
                    Directory.Delete(_currentPdfDirectory, true);
                }
                _currentPdfDirectory = value;
                OnPropertyChanged(nameof(CurrentPdfDirectory));
            }
        }
        public string CurrentPicturesDirectory
        {
            get => _currentPicturesDirectory;
            set
            {
                if (Directory.Exists(_currentPicturesDirectory))
                {
                    Directory.Delete(_currentPicturesDirectory, true);
                }
                _currentPicturesDirectory = value;
                OnPropertyChanged(nameof(CurrentPicturesDirectory));
            }
        }

        public string CurrentPdfFile
        {
            get => _currentPdfFile;
            set
            {
                _currentPdfFile = value;
                OnPropertyChanged(nameof(CurrentPdfFile));
            }
        }
        public ObservableCollection<ImageItem> Pictures { get; set; } = new ObservableCollection<ImageItem>();

        public async void OnDocumentChanged()
        {
            try
            {
                if (_navigationStore.CurrentViewModel is FormatViewModel formatViewModel)
                {
                    CurrentDocxDirectory = formatViewModel.SelectorCardViewModel.DocumentPath;
                }

                if (CurrentDocxDirectory == null) return;

                CurrentPdfDirectory = CreateTempPath();

                CurrentPdfFile = await Convert();
                CurrentPicturesDirectory = Directory.CreateDirectory(Path.Combine(CurrentPdfDirectory, "Pictures")).FullName;
                Pictures.Clear();
                var pictures = PdfToPngConverter.Convert(CurrentPdfFile, CurrentPicturesDirectory);

                foreach (var p in pictures)
                {
                    Pictures.Add(new ImageItem { Path = p, IsSelected = false });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при конвертации: {ex.Message}");
            }
        }

        private async Task<string> Convert()
        {
            _currentProcess?.Kill();
            
            if(CurrentDocxDirectory == null)
            {
                return null;
            }
                
            Priority priority = _options.Value.ConverterPriority;

            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PdfConverter.exe");

            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = $"\"{priority.ToString()}\" \"{CurrentDocxDirectory}\" \"{CurrentPdfDirectory}\"",
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
                throw new Exception(!string.IsNullOrWhiteSpace(error) ? error : $"Процесс завершился с кодом {_currentProcess.ExitCode}");
            }

            return result.Trim();
        }

        private string CreateTempPath()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);
            return tempFolder;
        }
    }
}
