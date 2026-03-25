using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class TableShortMenuViewModel : ViewModelBase
    {
        private TableStyle _tableStyle;
        private readonly TableService _tableService;
        private readonly AlignmentService _alignmentService;
        private readonly TextService _textService;
        private readonly ParagraphService _paragraphService;

        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;

        public TableShortMenuViewModel(TableService tableService, AlignmentService alignmentService, TextService textService, ParagraphService paragraphService,
            IServiceProvider provider, NavigationStore navigationStore)
        {
            _tableService = tableService;
            _alignmentService = alignmentService;
            _textService = textService;
            _paragraphService = paragraphService;

            _provider = provider;
            _navigationStore = navigationStore;

            DeleteTableStyleCommand = new AsyncRelayCommand(DeleteTableStyle, CanDelete);
            DetailCommand = new GoToDetailsCommand<TableStyleViewModel>(_provider, _navigationStore);
        }

        public ICommand DeleteTableStyleCommand { get; }
        public ICommand DetailCommand { get; }

        public TableStyle Style => _tableStyle;
        public string Name => _tableStyle.Name;
        public string Alignmnt => AlignmentToView(_alignmentService.GetById(_tableStyle.Alignment).GetAwaiter().GetResult().Alignment1);
        public string TextStyleName => _textService.GetById(_tableStyle.TextStyle).GetAwaiter().GetResult().Name;
        public string ParagraphStyleName => _paragraphService.GetById(_tableStyle.ParagraphStyle).GetAwaiter().GetResult().Name;
        public int BorderThikness => _tableStyle.BorderThikness;
        public string BorderColor => _tableStyle.BorderColor;

        public bool CanDelete(object? parametr)
        {
            return true;
        }

        public async Task DeleteTableStyle(object? paeametr)
        {
            await _tableService.Delete(_tableStyle);
        }

        private string AlignmentToView(string alignment)
        {
            switch (alignment.ToLower())
            {
                case "top":
                    return "По верху";
                case "bottom":
                    return "По низу";
                case "center":
                    return "По центру";
                default:
                    return "";

            }
        }

        public void Load(TableStyle style)
        {
            if (style != null)
            {
                _tableStyle = style;
            }
        }
    }
}
