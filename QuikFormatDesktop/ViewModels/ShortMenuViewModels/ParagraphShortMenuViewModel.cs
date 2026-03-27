using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class ParagraphShortMenuViewModel : ShortMenuViewModelBase
    {
        private ParagraphStyle _paragraphStyle;
        private readonly ParagraphService _paragraphService;
        private readonly AlignmentService _alignmentService;

        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;

        public ParagraphShortMenuViewModel(ParagraphService paragraphService, AlignmentService alignmentService, 
            IServiceProvider provider, NavigationStore navigationStore)
        {
            _paragraphService = paragraphService;
            _alignmentService = alignmentService;
            _provider = provider;
            _navigationStore = navigationStore;

            DeleteParagraphStyleCommand = new AsyncRelayCommand(DeleteParagraphStyle);
            DetailCommand = new RelayCommand(GoToDetailsWithAction);
        }

        public ICommand DeleteParagraphStyleCommand { get; }
        public ICommand DetailCommand { get; }

        public ParagraphStyle Style => _paragraphStyle;
        public string Name => _paragraphStyle.Name;
        public string Alignment => AlignmentToString(_alignmentService.GetById(_paragraphStyle.Alignment).GetAwaiter().GetResult().Alignment1);
        public double? LeftIndent => _paragraphStyle.LeftIndent;
        public double? RightIndent => _paragraphStyle.RightIndent;

        public double Interval => _paragraphStyle.IntervalInText;

        private string AlignmentToString(string alignmentType)
        {
            switch (alignmentType)
            {
                case "left":
                    return "По левому краю";
                case "right":
                    return "По правому краю";
                case "center":
                    return "По центру";
                case "both":
                    return "По ширине";
                default:
                    return "";
            }
        }

        private async Task DeleteParagraphStyle(object? parametr)
        {
            await _paragraphService.Delete(_paragraphStyle);
            ClosePopup?.Invoke();
        }

        public void Load(ParagraphStyle style)
        {
            if (style != null)
            {
                _paragraphStyle = style;
            }
        }

        private void GoToDetailsWithAction(object? parameter)
        {
            new GoToDetailsCommand<ParagraphStyleViewModel>(_provider, _navigationStore).Execute(parameter);
            ClosePopup?.Invoke();
        }
    }
}
