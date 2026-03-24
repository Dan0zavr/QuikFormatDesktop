using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.ShortMenuViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private readonly TemplateService _templateService;
        private readonly TextService _textService;
        private readonly ParagraphService _paragraphService;
        private readonly TableService _tableService;
        private readonly NumberingService _numberingService;
        private readonly PictureService _pictureService;
        private readonly FormulaService _formulaService;
        private readonly FontService _fontService;
        private readonly AlignmentService _alignmentService;
        private readonly PositionService _positionService;
        private readonly MarkerService _markerService;
        private object? _popupViewModel;

        private StyleObject _selectedItem;
        private ObservableCollection<StyleObject> _items = new ObservableCollection<StyleObject>();
        private string _groupBoxHeader;
        private bool _isPopupOpen;

        private StylesViewModel? _currentStylesViewModel;

        public NavigationViewModel(NavigationStore navigationStore,
            TemplateService templateService, TextService textService,
            ParagraphService paragraphService, TableService tableService,
            NumberingService numberingService, PictureService pictureService,
            FormulaService formulaService,
            NavigationService<StylesViewModel> navigationToStylesService,
            NavigationService<FormatViewModel> navigationToFormatService,
            FontService fontService,
            AlignmentService alignmentService,
            PositionService positionService, MarkerService markerService)
        {
            _navigationStore = navigationStore;
            _templateService = templateService;
            _textService = textService;
            _paragraphService = paragraphService;
            _tableService = tableService;
            _numberingService = numberingService;
            _pictureService = pictureService;
            _formulaService = formulaService;

            _fontService = fontService;
            _alignmentService = alignmentService;
            _positionService = positionService;
            _markerService = markerService;

            GoToStyles = new NavigateCommand<StylesViewModel>(navigationToStylesService);
            GoToFormat = new NavigateCommand<FormatViewModel>(navigationToFormatService);

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            ItemsView = CollectionViewSource.GetDefaultView(Items);
        }

        public ICommand GoToStyles { get; }
        public ICommand GoToFormat { get; }

        public ICollectionView ItemsView { get; }

        public StyleObject SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;

                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));

                if (_selectedItem != null)
                {
                    CreatePopupViewModel(_selectedItem);
                    IsPopupOpen = true;
                }
            }
        }
        

        public ObservableCollection<StyleObject> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public string GroupBoxHeader
        {
            get => _groupBoxHeader;
            set
            {
                _groupBoxHeader = value;
                OnPropertyChanged(nameof(GroupBoxHeader));
            }
        }

        public object? PopupViewModel
        {
            get => _popupViewModel;
            set
            {
                _popupViewModel = value;
                OnPropertyChanged(nameof(PopupViewModel));
            }
        }

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                if (_isPopupOpen == value)
                    return;

                _isPopupOpen = value;
                OnPropertyChanged(nameof(IsPopupOpen));
                if (!_isPopupOpen && SelectedItem != null)
                {
                    SelectedItem = null;
                }
            }
        }

        private async void OnCurrentViewModelChanged()
        {
            if (_currentStylesViewModel != null)
            {
                _currentStylesViewModel.PropertyChanged -= OnStylesPropertyChanged;
                _currentStylesViewModel = null;
            }

            await UpdateContent();

            if (_navigationStore.CurrentViewModel is StylesViewModel stylesVM)
            {
                _currentStylesViewModel = stylesVM;
                stylesVM.PropertyChanged += OnStylesPropertyChanged;
            }
        }
        private async void OnStylesPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StylesViewModel.SelectedTabIndex))
            {
                if (sender is StylesViewModel stylesVM)
                {
                    await UpdateStyles(stylesVM);
                }
            }
        }

        private async Task UpdateContent()
        {
            Items.Clear();

            if (_navigationStore.CurrentViewModel is FormatViewModel)
            {
                GroupBoxHeader = "Шаблоны";

                var templates = await _templateService.GetAll();

                foreach (var t in templates)
                {
                    t.Type = StyleType.None;
                    Items.Add(t);
                }
            }
            else if (_navigationStore.CurrentViewModel is StylesViewModel stylesVM)
            {
                await UpdateStyles(stylesVM);
            }

            UpdateGrouping();
        }

        private void UpdateGrouping()
        {
            ItemsView.GroupDescriptions.Clear();

            var distinctTypes = Items
                .Select(i => i.Type)
                .Where(t => t != StyleType.None)
                .Distinct()
                .Count();

            if (distinctTypes > 1)
            {
                ItemsView.GroupDescriptions.Add(
                    new PropertyGroupDescription(nameof(StyleObject.Type)));
            }
        }

        private async Task UpdateStyles(StylesViewModel stylesVM)
        {
            Items.Clear();

            switch (stylesVM.SelectedTab)
            {
                case TabItemIndex.Text:
                    GroupBoxHeader = "Стили текста";

                    var text = await _textService.GetAll();
                    foreach (var item in text)
                    {
                        item.Type = StyleType.Text;
                        Items.Add(item);
                    }

                    var paragraph = await _paragraphService.GetAll();
                    foreach (var item in paragraph)
                    {
                        item.Type = StyleType.Paragraph;
                        Items.Add(item);
                    }
                    break;

                case TabItemIndex.Numbering:
                    GroupBoxHeader = "Стили списков";

                    var markedNumbering = await _numberingService.GetStylesByType(MarkerTypeEnum.Marked);
                    foreach (var item in markedNumbering)
                    {
                        item.Type = StyleType.MarkedNumbering;
                        Items.Add(item);
                    }

                    var numberedNumbering = await _numberingService.GetStylesByType(MarkerTypeEnum.Numberd);
                    foreach (var item in numberedNumbering)
                    {
                        item.Type = StyleType.NumberedNumbering;
                        Items.Add(item);
                    }
                    break;

                case TabItemIndex.Table:
                    GroupBoxHeader = "Стили таблиц";

                    var tables = await _tableService.GetAll();
                    foreach (var item in tables)
                        Items.Add(item);
                    break;

                case TabItemIndex.Picture:
                    GroupBoxHeader = "Стили рисунков";

                    var pictures = await _pictureService.GetAll();
                    foreach (var item in pictures)
                        Items.Add(item);
                    break;

                case TabItemIndex.Formula:
                    GroupBoxHeader = "Стили формул";

                    var formulas = await _formulaService.GetAll();
                    foreach (var item in formulas)
                        Items.Add(item);
                    break;
            }

            UpdateGrouping();
        }

        private void CreatePopupViewModel(StyleObject style)
        {
            switch (style)
            {
                case TextStyle text:
                    PopupViewModel = new TextShortMenuViewModel(text, _textService, _fontService);
                    break;

                case ParagraphStyle paragraph:
                    PopupViewModel = new ParagraphShortMenuViewModel(paragraph, _paragraphService, _alignmentService);
                    break;

                case TableStyle table:
                    PopupViewModel = new TableShortMenuViewModel(table, _tableService, _alignmentService, _textService, _paragraphService);
                    break;

                case NumberingStyle numbering:
                    PopupViewModel = new NumberingShortMenuViewModel(numbering, _numberingService, _markerService);
                    break;

                case PictureStyle picture:
                    PopupViewModel = new PictureShortMenuViewModel(picture, _pictureService, _paragraphService);
                    break;

                case FormulaStyle formula:
                    PopupViewModel = new FormulaShortMenuViewModel(formula, _formulaService, _positionService, _markerService);
                    break;

                default:
                    PopupViewModel = null;
                    break;
            }
        }
    }
}
