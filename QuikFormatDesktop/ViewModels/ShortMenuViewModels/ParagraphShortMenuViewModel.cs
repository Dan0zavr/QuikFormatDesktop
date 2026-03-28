using CommunityToolkit.Mvvm.Input;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Commands.ModalCommands;
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
        private ModalNavigationService<DeleteWarningViewModel> _warningService;

        public ParagraphShortMenuViewModel(ParagraphService paragraphService, AlignmentService alignmentService,
            IServiceProvider provider, NavigationStore navigationStore, ModalNavigationService<DeleteWarningViewModel> warningService)
        {
            _paragraphService = paragraphService;
            _alignmentService = alignmentService;
            _provider = provider;
            _navigationStore = navigationStore;
            _warningService = warningService;

            DeleteParagraphStyleCommand = new RelayCommand<object?>(OpenDeleteWarning);
            DetailCommand = new RelayCommand<object?>(GoToDetailsWithAction);
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

        private void OpenDeleteWarning(object? parameter)
        {
            new OpenDeleteWarningCommand(_warningService).Execute(parameter);
            if (_navigationStore.CurrentModalViewModel is DeleteWarningViewModel deleteWarning)
            {
                ClosePopup?.Invoke();
                deleteWarning.Load(_paragraphStyle);
                deleteWarning.DeleteCommand = new AsyncRelayCommand<object?>(DeleteParagraphStyle);
            }
        }
    }
}
