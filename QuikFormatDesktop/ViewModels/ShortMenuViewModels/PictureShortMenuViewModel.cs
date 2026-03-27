using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using QuikFormatDesktop.ViewModels.StylesViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class PictureShortMenuViewModel : ShortMenuViewModelBase
    {
        private PictureStyle _pictureStyle;
        private readonly PictureService _pictureService;
        private readonly ParagraphService _paragraphService;

        private readonly IServiceProvider _provider;
        private readonly NavigationStore _navigationStore;

        public PictureShortMenuViewModel(PictureService pictureService, ParagraphService paragraphService, 
            IServiceProvider provider, NavigationStore navigationStore)
        {
            _pictureService = pictureService;
            _paragraphService = paragraphService;
            _provider = provider;
            _navigationStore = navigationStore;

            DeletePictureStyleCommand = new AsyncRelayCommand(DeletePictureStyle);
            DetailCommand = new RelayCommand(GoToDetailsWithAction);
        }

        public ICommand DeletePictureStyleCommand { get; }
        public ICommand DetailCommand { get; }

        public PictureStyle Style => _pictureStyle;
        public string Name => _pictureStyle.Name;
        public string ParagraphStyleName => _paragraphService.GetById(_pictureStyle.ParagraphStyle).GetAwaiter().GetResult().Name;
        public bool GenerateLabel => _pictureStyle.GenerateLabel;
        public string LabelValue => _pictureStyle.LabelValue;
        public bool EmptyLineAround => _pictureStyle.EmptyLineAround;

        private async Task DeletePictureStyle(object? parametr)
        {
            await _pictureService.Delete(_pictureStyle);
            ClosePopup?.Invoke();
        }

        public void Load(PictureStyle style)
        {
            if (style != null)
            {
                _pictureStyle = style;
            }
        }

        private void GoToDetailsWithAction(object? parameter)
        {
            new GoToDetailsCommand<PictureStyleViewModel>(_provider, _navigationStore).Execute(parameter);
            ClosePopup?.Invoke();
        }
    }
}
