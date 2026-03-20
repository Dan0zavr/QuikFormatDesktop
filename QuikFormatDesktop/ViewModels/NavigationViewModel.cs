using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;

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

        private object _selectedItem;
        private ObservableCollection<object> _items = new ObservableCollection<object>();
        private string _groupBoxHeader;

        public NavigationViewModel(NavigationStore navigationStore, 
            TemplateService templateService, TextService textService, 
            ParagraphService paragraphService, TableService tableService, 
            NumberingService numberingService, PictureService pictureService,
            FormulaService formulaService,
            NavigationService<StylesViewModel> navigationToStylesService, NavigationService<FormatViewModel> navigationToFormatService)
        {
            _navigationStore = navigationStore;
            _templateService = templateService;
            _textService = textService;
            _paragraphService = paragraphService;
            _tableService = tableService;
            _numberingService = numberingService;
            _pictureService = pictureService;
            _formulaService = formulaService;

            GoToStyles = new NavigateCommand<StylesViewModel>(navigationToStylesService);
            GoToFormat = new NavigateCommand<FormatViewModel>(navigationToFormatService);

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        public ICommand GoToStyles { get; }
        public ICommand GoToFormat { get; }

        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ObservableCollection<object> Items
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

        private void OnCurrentViewModelChanged()
        {
            UpdateContent();

            if (_navigationStore.CurrentViewModel is StylesViewModel stylesVM)
            {
                stylesVM.PropertyChanged += OnStylesPropertyChanged;
            }
        }
        private void OnStylesPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StylesViewModel.SelectedTabIndex))
            {
                if (sender is StylesViewModel stylesVM)
                {
                    UpdateStyles(stylesVM);
                }
            }
        }

        private async void UpdateContent()
        {
            Items.Clear();

            if (_navigationStore.CurrentViewModel is FormatViewModel)
            {
                GroupBoxHeader = "Шаблоны";

                var templates = await _templateService.GetAll();

                foreach (var t in templates)
                    Items.Add(t);
            }
            else if (_navigationStore.CurrentViewModel is StylesViewModel stylesVM)
            {
                UpdateStyles(stylesVM);
            }
        }

        private async void UpdateStyles(StylesViewModel stylesVM)
        {
            Items.Clear();

            switch (stylesVM.SelectedTab)
            {
                case TabItemIndex.Text:
                    GroupBoxHeader = "Стили текста";

                    var text = await _textService.GetAll();
                    foreach (var item in text)
                        Items.Add(item);
                    break;

                case TabItemIndex.Numbering:
                    GroupBoxHeader = "Стили списков";

                    var numbering = await _numberingService.GetAll();
                    foreach (var item in numbering)
                        Items.Add(item);
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
        }
    }
}
