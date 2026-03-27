using Microsoft.Web.WebView2.Core;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class TemplateViewModel : ViewModelBase
    {
        private const string NO_STYLE = "Нет стиля";

        private string _templateName;
        private string _templateDescription;

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

        private readonly AlignmentService _alignmentService;
        private readonly PositionService _positionService;
        private readonly MarkerTypeService _markerTypeService; 
        private readonly MarkerService _markerService;

        private NavigationStore _navigationStore;

        public TemplateViewModel(TextService textService, ParagraphService paragraphService, TableService tableService,
            PictureService pictureService, NumberingService numberingService, FormulaService formulaService, TemplateService templateService,
            AlignmentService alignmentService, PositionService positionService, MarkerTypeService markerTypeService, MarkerService markerService, 
            NavigationStore navigationStore)
        {
            _textService = textService;
            _paragraphService = paragraphService;
            _tableService = tableService;
            _pictureService = pictureService;
            _numberingService = numberingService;
            _formulaService = formulaService;
            _templateService = templateService;
            _alignmentService = alignmentService;
            _positionService = positionService;
            _markerTypeService = markerTypeService;
            _markerService = markerService;
            _navigationStore = navigationStore;

            CloseModalCommand = new RelayCommand(CloseModal);
            AddTemplateCommand = new AsyncRelayCommand(AddTemplate, CanAdd);
        }

        public ICommand CloseModalCommand { get; }
        public ICommand AddTemplateCommand { get; }

        public string TemplateName {
            get => _templateName;
            set
            {
                _templateName = value;
                OnPropertyChanged(nameof(TemplateName));
                (AddTemplateCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

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
            List<NumberingStyle> numberedNumbering = await _numberingService.GetStylesByType(Enums.MarkerTypeEnum.Numberd);

            FillCollection(MarkedNumberingStyleItems, markedNumbering, includeEmpty: true);
            FillCollection(NumberedNumberingStyleItems, numberedNumbering, includeEmpty: true);
        }

        private async Task LoadFormulaStyles()
        {
            List<FormulaStyle> styles = await _formulaService.GetAll();
            FillCollection(FormulaStyleItems, styles, includeEmpty: true);
        }

        public async Task LoadData()
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
                }
            }
            finally
            {
                CloseModal(parameter);
            }
        }

    }
}
