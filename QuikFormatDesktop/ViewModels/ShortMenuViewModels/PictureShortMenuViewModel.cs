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
    public class PictureShortMenuViewModel : ViewModelBase
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

            DeletePictureStyleCommand = new AsyncRelayCommand(DeletePictureStyle, CanDelete);
            DetailCommand = new GoToDetailsCommand<PictureStyleViewModel>(_provider, _navigationStore);
        }

        public ICommand DeletePictureStyleCommand { get; }
        public ICommand DetailCommand { get; }

        public PictureStyle Style => _pictureStyle;
        public string Name => _pictureStyle.Name;
        public string ParagraphStyleName => _paragraphService.GetById(_pictureStyle.ParagraphStyle).GetAwaiter().GetResult().Name;
        public bool GenerateLabel => _pictureStyle.GenerateLabel;
        public string LabelValue => _pictureStyle.LabelValue;
        public bool EmptyLineAround => _pictureStyle.EmptyLineAround;

        private bool CanDelete(object? parametr)
        {
            return true;
        }

        private async Task DeletePictureStyle(object? parametr)
        {
            await _pictureService.Delete(_pictureStyle);
        }

        public void Load(PictureStyle style)
        {
            if (style != null)
            {
                _pictureStyle = style;
            }
        }
    }
}
