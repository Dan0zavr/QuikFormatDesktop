using CommunityToolkit.Mvvm.Input;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class TemplateViewModel : ViewModelBase
    {
        private const string NO_STYLE = "Нет стиля";

        private string _templateName;
        private string _templateDescription;
        private bool _isEdit;

        private TextStyle _selectedTextStyle;
        private ParagraphStyle _selectedParagraphStyle;
        private TableStyle _selectedTableStyle;
        private PictureStyle _selectedPictureStyle;
        private NumberingStyle _selectedMarkedNumberingStyle;
        private NumberingStyle _selectedNumberedNumberingStyle;
        private FormulaStyle _selectedFormulaStyle;

        private readonly TextService _textService;
        private readonly ParagraphService _paragraphService;
        private readonly TableService _tableService;
        private readonly PictureService _pictureService;
        private readonly NumberingService _numberingService;
        private readonly FormulaService _formulaService;
        private readonly TemplateService _templateService;

        private bool _isPopupOpen = false;
        private string _popupMessage;
        private Color _popupBackground;
        private Color _popupForeground;

        private NavigationStore _navigationStore;

        public TemplateViewModel(TextService textService, ParagraphService paragraphService, TableService tableService,
            PictureService pictureService, NumberingService numberingService, FormulaService formulaService, TemplateService templateService,
            NavigationStore navigationStore)
        {
            _textService = textService;
            _paragraphService = paragraphService;
            _tableService = tableService;
            _pictureService = pictureService;
            _numberingService = numberingService;
            _formulaService = formulaService;
            _templateService = templateService;
            _navigationStore = navigationStore;

            CloseModalCommand = new RelayCommand<object?>(CloseModal);
            AddTemplateCommand = new AsyncRelayCommand<object?>(AddTemplate, CanAdd);
            UpdateTemplateCommand = new AsyncRelayCommand<object?>(UpdateTemplate, CanAdd);
        }

        public ICommand CloseModalCommand { get; }
        public ICommand AddTemplateCommand { get; }
        public ICommand UpdateTemplateCommand { get; }

        public bool IsLoading { get; private set; }

        public bool IsEdit
        {
            get => _isEdit;
            set
            {
                _isEdit = value;
            }
        }

        public string Title => IsEdit ? "Редактирование шаблона" : "Создание шаблона";

        private int TemplateId { get; set; }

        public string TemplateName {
            get => _templateName;
            set
            {
                _templateName = value;
                OnPropertyChanged(nameof(TemplateName));
                (AddTemplateCommand as IAsyncRelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        private string OldTemplateName { get; set; }

        public string TemplateDescription { 
            get => _templateDescription; 
            set
            {
                _templateDescription = value;
                OnPropertyChanged(nameof(TemplateDescription));
            }
        }

        public TextStyle SelectedTextStyle { 
            get => _selectedTextStyle;
            set
            {
                _selectedTextStyle = value;
                OnPropertyChanged(nameof(SelectedTextStyle));
            }
        }

        public ParagraphStyle SelectedParagraphStyle { 
            get => _selectedParagraphStyle;
            set
            {
                _selectedParagraphStyle = value;
                OnPropertyChanged(nameof(SelectedParagraphStyle));
            }
        }

        public TableStyle SelectedTableStyle { 
            get => _selectedTableStyle;
            set
            {
                _selectedTableStyle = value;
                OnPropertyChanged(nameof(SelectedTableStyle));
            }
        }

        public PictureStyle SelectedPictureStyle { 
            get => _selectedPictureStyle;
            set
            {
                _selectedPictureStyle = value;
                OnPropertyChanged(nameof(SelectedPictureStyle));
            }
        }

        public NumberingStyle SelectedMarkedNumberingStyle { 
            get => _selectedMarkedNumberingStyle;
            set
            {
                _selectedMarkedNumberingStyle = value;
                OnPropertyChanged(nameof(SelectedMarkedNumberingStyle));
            }
        }

        public NumberingStyle SelectedNumberedNumberingStyle {
            get => _selectedNumberedNumberingStyle;
            set
            {
                _selectedNumberedNumberingStyle = value;
                OnPropertyChanged(nameof(SelectedNumberedNumberingStyle));
            }
        }

        public FormulaStyle SelectedFormulaStyle
        {
            get => _selectedFormulaStyle;
            set
            {
                _selectedFormulaStyle = value;
                OnPropertyChanged(nameof(SelectedFormulaStyle));
            }
        }

        public ObservableCollection<StyleItem> TextStyleItems { get; set; } = new();
        public ObservableCollection<StyleItem> ParagraphStyleItems { get; set; } = new();
        public ObservableCollection<StyleItem> TableStyleItems { get; set; } = new();
        public ObservableCollection<StyleItem> PictureStyleItems { get; set; } = new();
        public ObservableCollection<StyleItem> MarkedNumberingStyleItems { get; set; } = new();
        public ObservableCollection<StyleItem> NumberedNumberingStyleItems { get; set; } = new();
        public ObservableCollection<StyleItem> FormulaStyleItems { get; set; } = new();

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

        private async Task LoadTextStyles()
        {
            List<TextStyle> styles = await _textService.GetAll();
            FillCollection(TextStyleItems, styles);
        }

        private async Task LoadParagraphStyles()
        {
            List<ParagraphStyle> styles = await _paragraphService.GetAll();
            FillCollection(ParagraphStyleItems, styles);
        }

        private async Task LoadTableStyles()
        {
            List<TableStyle> styles = await _tableService.GetAll();
            FillCollection(TableStyleItems, styles, includeEmpty: true);
        }

        private async Task LoadPictureStyles()
        {
            List<PictureStyle> styles = await _pictureService.GetAll();
            FillCollection(PictureStyleItems, styles, includeEmpty: true);
        }

        private async Task LoadNumberingStyles()
        {
            List<NumberingStyle> markedNumbering = await _numberingService.GetStylesByType(Enums.MarkerTypeEnum.Marked);
            List<NumberingStyle> numberedNumbering = await _numberingService.GetStylesByType(Enums.MarkerTypeEnum.Numbered);

            FillCollection(MarkedNumberingStyleItems, markedNumbering, includeEmpty: true);
            FillCollection(NumberedNumberingStyleItems, numberedNumbering, includeEmpty: true);
        }

        private async Task LoadFormulaStyles()
        {
            List<FormulaStyle> styles = await _formulaService.GetAll();
            FillCollection(FormulaStyleItems, styles, includeEmpty: true);
        }

        public async Task LoadStylesData()
        {
            await Task.WhenAll(
                LoadTextStyles(),
                LoadParagraphStyles(),
                LoadTableStyles(),
                LoadPictureStyles(),
                LoadNumberingStyles(),
                LoadFormulaStyles());
        }

        public void SetDefault()
        {
            if (!IsEdit)
            {
                TemplateName = string.Empty;
                TemplateDescription = string.Empty;

                SelectedTextStyle = TextStyleItems.FirstOrDefault()?.Style as TextStyle;

                SelectedParagraphStyle = ParagraphStyleItems.FirstOrDefault()?.Style as ParagraphStyle;

                SelectedTableStyle = TableStyleItems.FirstOrDefault()?.Style as TableStyle;

                SelectedPictureStyle = PictureStyleItems.FirstOrDefault()?.Style as PictureStyle;

                SelectedMarkedNumberingStyle = MarkedNumberingStyleItems.FirstOrDefault()?.Style as NumberingStyle;

                SelectedNumberedNumberingStyle = NumberedNumberingStyleItems.FirstOrDefault()?.Style as NumberingStyle;

                SelectedFormulaStyle = FormulaStyleItems.FirstOrDefault()?.Style as FormulaStyle;
            }
        }

        private void FillCollection<T>(ObservableCollection<StyleItem> collection, IEnumerable<T> styles, bool includeEmpty = false) where T : StyleObject
        {
            collection.Clear();

            if (includeEmpty)
            {
                collection.Add(new StyleItem
                {
                    DisplayName = NO_STYLE,
                    Style = null
                });
            }

            foreach (var style in styles)
            {
                collection.Add(new StyleItem
                {
                    DisplayName = style.Name,
                    Style = style
                });
            }
        }

        private void CloseModal(object? parameter)
        {
            _navigationStore.CurrentModalViewModel = null;
        }

        private bool CanAdd(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(TemplateName))
            {
                return true;
            }
            return false;
        }

        private async Task AddTemplate(object? parameter)
        {
            try
            {
                Template template = new Template
                {
                    Name = TemplateName,
                    Description = TemplateDescription,
                    TextStyle = SelectedTextStyle.Id,
                    ParagraphStyle = SelectedParagraphStyle.Id,
                    TableStyle = SelectedTableStyle?.Id,
                    NumberedNumberingStyle = SelectedNumberedNumberingStyle?.Id,
                    MarkedNumberingStyle = SelectedMarkedNumberingStyle?.Id,
                    PictureStyle = SelectedPictureStyle?.Id,
                    FormulaStyle = SelectedFormulaStyle?.Id
                };

                if (await _templateService.IsUnique(template.Name))
                {
                    await _templateService.Add(template);
                    await ShowPopup("Шаблон успешно добавлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Шаблон с таким именем уже существует", PopupType.Bad);
                }
            }
            finally
            {
                CloseModal(parameter);
            }
        }

        private async Task UpdateTemplate(object? parameter)
        {
            try
            {
                Template template = new Template
                {
                    Id = TemplateId,
                    Name = TemplateName,
                    Description = TemplateDescription,
                    TextStyle = SelectedTextStyle.Id,
                    ParagraphStyle = SelectedParagraphStyle.Id,
                    TableStyle = SelectedTableStyle?.Id,
                    NumberedNumberingStyle = SelectedNumberedNumberingStyle?.Id,
                    MarkedNumberingStyle = SelectedMarkedNumberingStyle?.Id,
                    PictureStyle = SelectedPictureStyle?.Id,
                    FormulaStyle = SelectedFormulaStyle?.Id
                };

                bool isUniqe = true;

                if(OldTemplateName != template.Name)
                {
                    isUniqe = await _templateService.IsUnique(template.Name);
                }

                if (isUniqe)
                {
                    await _templateService.Update(template);
                    await ShowPopup("Шаблон успешно обновлен", PopupType.Good);
                }
                else
                {
                    await ShowPopup("Шаблон с таким именем уже существует", PopupType.Bad);
                }
            }
            finally
            {
                CloseModal(parameter);
            }
        }

        public void LoadTemplateData(object parametr, bool isEdit = false)
        {
            if(parametr is Template template)
            {
                IsEdit = isEdit;
                TemplateId = template.Id;
                TemplateName = template.Name;
                OldTemplateName = template.Name;
                TemplateDescription = template.Description;
                SelectedTextStyle = TextStyleItems.FirstOrDefault(x => x.Style?.Id == template.TextStyle)?.Style as TextStyle;
                SelectedParagraphStyle = ParagraphStyleItems.FirstOrDefault(x => x.Style?.Id == template.ParagraphStyle)?.Style as ParagraphStyle;
                SelectedTableStyle = TableStyleItems.FirstOrDefault(x => x.Style?.Id == template.TableStyle)?.Style as TableStyle;
                SelectedPictureStyle = PictureStyleItems.FirstOrDefault(x => x.Style?.Id == template.PictureStyle)?.Style as PictureStyle;
                SelectedMarkedNumberingStyle = MarkedNumberingStyleItems.FirstOrDefault(x => x.Style?.Id == template.MarkedNumberingStyle)?.Style as NumberingStyle;
                SelectedNumberedNumberingStyle = NumberedNumberingStyleItems.FirstOrDefault(x => x.Style?.Id == template.NumberedNumberingStyle)?.Style as NumberingStyle;
                SelectedFormulaStyle = FormulaStyleItems.FirstOrDefault(x => x.Style?.Id == template.FormulaStyle)?.Style as FormulaStyle;
            }
        }

        public async Task InitializeAsync(Template template = null, bool isEdit = false)
        {
            IsLoading = true;
            OnPropertyChanged(nameof(IsLoading));

            try
            {
                await LoadStylesData();

                if (template != null)
                {
                    LoadTemplateData(template, isEdit);
                }
                else
                {
                    SetDefault();
                }
            }
            finally
            {
                IsLoading = false;
                OnPropertyChanged(nameof(IsLoading));
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
