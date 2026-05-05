using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using PDFReader;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.RightsManagement;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace QuikFormatDesktop.ViewModels.FormatViewModels
{
    public class SelectorCardViewModel : ViewModelBase
    {
        private readonly TextService _textService;
        private readonly ParagraphService _paragraphService;
        private readonly NumberingService _numberingService;
        private readonly TableService _tableService;
        private readonly PictureService _pictureService;
        private readonly FormulaService _formulaService;
        private readonly NavigationStore _navigationStore;
        private readonly IOptions<SystemStyles> _systemStyles;

        private Template _template;

        private string _templateName;
        private string _templateDescription;

        private TextStyle _textStyle;
        private ParagraphStyle _paragraphStyle;
        private TableStyle _tableStyle;
        private PictureStyle _pictureStyle;
        private NumberingStyle _markedNumberingStyle;
        private NumberingStyle _numberedNumberingStyle;
        private FormulaStyle _formulaStyle;

        private bool _isPreviewVisible;

        private string _documentPath;

        private string _saveDirectory;

        private string _popupMessage;
        private bool _isPopupOpen = false;
        private Color _popupBackground;
        private Color _popupForeground;

        public event Action? DocumentChanged;
        public event Action? TemplateChanged;

        public SelectorCardViewModel(TextService textService, ParagraphService paragraphService,
            NumberingService numberingService, TableService tableService, PictureService pictureService,
            FormulaService formulaService, NavigationStore navigationStore, IOptions<SystemStyles> systemStyles)
        {
            _textService = textService;
            _paragraphService = paragraphService;
            _numberingService = numberingService;
            _tableService = tableService;
            _pictureService = pictureService;
            _formulaService = formulaService;
            _navigationStore = navigationStore;
            _systemStyles = systemStyles;

            CleanTemplateCommand = new RelayCommand(CleanTemplate);
            OpenFileDialogCommand = new RelayCommand(OpenFileDialog);
            CleanDocumentCommand = new RelayCommand(CleanDocument);
            DropDocumentCommand = new RelayCommand<DragEventArgs>(DropDocument);
            DragOverCommand = new RelayCommand<DragEventArgs>(DragOver);
            OpenDirectoryDialogCommand = new RelayCommand(OpenDirectoryDialog);
            FormatCommand = new AsyncRelayCommand(SaveFormattedDocument, CanFormat);
        }

        public ICommand CleanTemplateCommand { get; }
        public ICommand OpenFileDialogCommand { get; }
        public ICommand CleanDocumentCommand { get; }
        public ICommand DropDocumentCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand OpenDirectoryDialogCommand { get; }
        public ICommand FormatCommand { get; }

        public bool IsTemplateSelected => _template != null;
        public Template Template
        {
            get => _template;
            set
            {
                _template = value;
                OnPropertyChanged(nameof(Template));
                OnPropertyChanged(nameof(IsTemplateSelected));
                (FormatCommand as IRelayCommand)?.NotifyCanExecuteChanged();
                TemplateChanged?.Invoke();
            }
        }
        public string TemplateName
        {
            get => _templateName;
            set
            {
                _templateName = value;
                OnPropertyChanged(nameof(TemplateName));
            }
        }
        public string Description
        {
            get => _templateDescription;
            set
            {
                _templateDescription = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        public TextStyle TextStyle {
            get => _textStyle;
            set
            {
                _textStyle = value;
                OnPropertyChanged(nameof(TextStyle));
                OnPropertyChanged(nameof(IsTextStyleVisible));
            }
        }
        public ParagraphStyle ParagraphStyle
        {
            get => _paragraphStyle;
            set
            {
                _paragraphStyle = value;
                OnPropertyChanged(nameof(ParagraphStyle));
                OnPropertyChanged(nameof(IsParagraphStyleVisible));
            }
        }
        public TableStyle TableStyle
        {
            get => _tableStyle;
            set
            {
                _tableStyle = value;
                OnPropertyChanged(nameof(TableStyle));
                OnPropertyChanged(nameof(IsTableStyleVisible));
            }
        }
        public PictureStyle PictureStyle
        {
            get => _pictureStyle;
            set
            {
                _pictureStyle = value;
                OnPropertyChanged(nameof(PictureStyle));
                OnPropertyChanged(nameof(IsPictureStyleVisible));
            }
        }
        public NumberingStyle MarkedNumberingStyle
        {
            get => _markedNumberingStyle;
            set
            {
                _markedNumberingStyle = value;
                OnPropertyChanged(nameof(MarkedNumberingStyle));
                OnPropertyChanged(nameof(IsMarkedNumberingStyleVisible));
            }
        }
        public NumberingStyle NumberedNumberingStyle
        {
            get => _numberedNumberingStyle;
            set
            {
                _numberedNumberingStyle = value;
                OnPropertyChanged(nameof(NumberedNumberingStyle));
                OnPropertyChanged(nameof(IsNumberedNumberingStyleVisible));
            }
        }
        public FormulaStyle FormulaStyle
        {
            get => _formulaStyle;
            set
            {
                _formulaStyle = value;
                OnPropertyChanged(nameof(FormulaStyle));
                OnPropertyChanged(nameof(IsFormulaStyleVisible));
            }
        }
        public bool IsTextStyleVisible => TextStyle == null ? false : true;
        public bool IsParagraphStyleVisible => ParagraphStyle == null ? false : true;
        public bool IsTableStyleVisible => TableStyle == null ? false : true;
        public bool IsNumberedNumberingStyleVisible => NumberedNumberingStyle == null ? false : true;
        public bool IsMarkedNumberingStyleVisible => MarkedNumberingStyle == null ? false : true;
        public bool IsPictureStyleVisible => PictureStyle == null ? false : true;
        public bool IsFormulaStyleVisible => FormulaStyle == null ? false : true;
        
        public bool IsPreviewVisible
        {
            get => _isPreviewVisible;
            set
            {
                if (_isPreviewVisible != value)
                {
                    _isPreviewVisible = value;
                    OnPropertyChanged(nameof(IsPreviewVisible));
                }
            }
        }

        public bool IsDocumentSelected => DocumentPath == null ? false : true;

        public string DocumentPath { 
            get => _documentPath;
            set
            {
                _documentPath = value;
                OnPropertyChanged(nameof(DocumentPath));
                OnPropertyChanged(nameof(DisplayDocumentPath));
                OnPropertyChanged(nameof(IsDocumentSelected));
                (FormatCommand as IRelayCommand)?.NotifyCanExecuteChanged();
                DocumentChanged?.Invoke();
            }
        }

        public string DisplayDocumentPath => CutPath(DocumentPath);

        public string SaveDirectory
        {
            get => _saveDirectory;
            set
            {
                _saveDirectory = value;
                OnPropertyChanged(nameof(SaveDirectory));
                OnPropertyChanged(nameof(DisplayDirectory));
                (FormatCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public string DisplayDirectory => CutPath(SaveDirectory);

        public string PopupMessage
        {
            get => _popupMessage;
            set
            {
                _popupMessage = value;
                OnPropertyChanged(nameof(PopupMessage));
            }
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
            }
        }

        public Color PopupBackground
        {
            get => _popupBackground;
            set
            {
                _popupBackground = value;
                OnPropertyChanged(nameof(PopupBackground));
            }
        }

        public Color PopupForeground
        {
            get => _popupForeground;
            set
            {
                _popupForeground = value;
                OnPropertyChanged(nameof(PopupForeground));
            }
        }

        public async Task LoadTemplate(Template template)
        {
            Template = template;
            TemplateName = _template.Name;
            Description = _template.Description;
            if(_template.TextStyle != null) TextStyle = await _textService.GetById((int)_template.TextStyle);
            if(_template.ParagraphStyle != null) ParagraphStyle = await _paragraphService.GetById((int)_template.ParagraphStyle);
            if(_template.TableStyle != null) TableStyle = await _tableService.GetById((int)_template.TableStyle);
            if (_template.NumberedNumberingStyle != null) NumberedNumberingStyle = await _numberingService.GetById((int)_template.NumberedNumberingStyle);
            if (_template.MarkedNumberingStyle != null) MarkedNumberingStyle = await _numberingService.GetById((int)_template.MarkedNumberingStyle);
            if(_template.PictureStyle != null) PictureStyle = await _pictureService.GetById((int)_template.PictureStyle);
            if(_template.FormulaStyle != null) FormulaStyle = await _formulaService.GetById((int)_template.FormulaStyle);
        }

        public async Task LoadSystemTemplate(Template template)
        {
            Template = template;
            TemplateName = _template.Name;
            Description = _template.Description;
            if (_template.TextStyle != null) TextStyle = _systemStyles.Value.TextStyles.FirstOrDefault(x => x.Id == template.TextStyle);
            if (_template.ParagraphStyle != null) ParagraphStyle = _systemStyles.Value.ParagraphStyles.FirstOrDefault(x => x.Id == template.ParagraphStyle);
            if (_template.TableStyle != null) TableStyle = _systemStyles.Value.TableStyles.FirstOrDefault(x => x.Id == template.TableStyle);
            if (_template.PictureStyle != null) PictureStyle = _systemStyles.Value.PictureStyles.FirstOrDefault(x => x.Id == template.PictureStyle);
            if (_template.NumberedNumberingStyle != null) NumberedNumberingStyle = _systemStyles.Value.NumberingStyles.FirstOrDefault(x => x.Id == template.NumberedNumberingStyle);
            if (_template.MarkedNumberingStyle != null) MarkedNumberingStyle = _systemStyles.Value.NumberingStyles.FirstOrDefault(x => x.Id == template.MarkedNumberingStyle);
            if (_template.FormulaStyle != null) FormulaStyle = _systemStyles.Value.FormulaStyles.FirstOrDefault(x => x.Id == template.FormulaStyle);
        }

        private void CleanTemplate()
        {
            Template = null;
            IsPreviewVisible = false;
        }

        private void CleanDocument()
        {
            DocumentPath = null;
        }

        private string CutPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath)) return string.Empty;

            string[] parts_ = fullPath.Split(new char[] { '/', '\\' });

            List<string> parts = parts_.ToList();

            List<int> deletedIndexes = new List<int>();

            while (parts.Sum(x => x.Length) > 35)
            {
                if (parts.Count == 2) break;

                int index = (parts.Count() - 1) / 2;
                parts.RemoveAt(index);
                deletedIndexes.Add(index);
            }

            string display = "";
            bool isDotsAdded = false;
            for (int i = 0; i < parts.Count; i++)
            {
                if (deletedIndexes.Contains(i))
                {
                    if (!isDotsAdded)
                    {
                        display += "...\\";
                        isDotsAdded = true;
                    }
                    deletedIndexes.Remove(i);
                    i--;
                }
                else
                {
                    display += parts[i] + "\\";
                }
            }
            display = display.TrimEnd('\\');
            return display;
        }

        private void OpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                openFileDialog.Filter = "DOCX Files (*.docx)|*.docx";
                DocumentPath = openFileDialog.FileName;
            }
        }

        private void OpenDirectoryDialog()
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            if (openFolderDialog.ShowDialog() == true)
            {
                SaveDirectory = openFolderDialog.FolderName;
            }
        }

        private void DragOver(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.None;
            else
                e.Effects = DragDropEffects.Copy;

            e.Handled = true;
        }

        private void DropDocument(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length > 0)
            {
                DocumentPath = files[0];
            }
        }

        private bool CanFormat()
        {
            if (Template != null && !string.IsNullOrEmpty(DocumentPath) && !string.IsNullOrEmpty(SaveDirectory))
            {
                return true;
            }
            return false;
        }

        private async Task SaveFormattedDocument()
        {
            if(_navigationStore.CurrentViewModel is FormatViewModel formatViewModel)
            {
                var previewViiewModel = formatViewModel.PreviewViewModel;
                string docPath = previewViiewModel.CurrentFormattedDocxFile;
                string newDocName = XMLParser.XMLWrite.EnsureUniqueFileName(Path.GetFileName(docPath), SaveDirectory);
                try
                {
                    File.Copy(docPath, Path.Combine(SaveDirectory, Path.GetFileName(newDocName)));
                    await ShowPopup("Документ успешно отформатирован в папке назначения", PopupType.Good);
                }
                catch(Exception ex)
                {
                    await ShowPopup($"Произошла ошибка при форматировании документа: {ex}", PopupType.Bad);
                }
            }
        }

        private async Task ShowPopup(string message, PopupType type)
        {
            switch (type)
            {
                case PopupType.Bad:
                    PopupBackground = (Color)ColorConverter.ConvertFromString("#fc9d9d");
                    PopupForeground = (Color)ColorConverter.ConvertFromString("#570000");
                    break;
                case PopupType.Good:
                    PopupBackground = (Color)ColorConverter.ConvertFromString("#b1ffa8");
                    PopupForeground = (Color)ColorConverter.ConvertFromString("#085200");
                    break;
                default:
                    PopupBackground = Colors.White;
                    PopupForeground = Colors.Black;
                    break;
            }

            PopupMessage = message;
            IsPopupOpen = true;
            await Task.Delay(2000);
            IsPopupOpen = false;
        }
    }
}
