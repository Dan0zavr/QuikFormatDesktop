using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class PictureShortMenuViewModel : ViewModelBase
    {
        private readonly PictureStyle _pictureStyle;
        private readonly PictureService _pictureService;
        private readonly ParagraphService _paragraphService;

        public PictureShortMenuViewModel(PictureStyle pictureStyle, PictureService pictureService, ParagraphService paragraphService)
        {
            _pictureStyle = pictureStyle;
            _pictureService = pictureService;
            _paragraphService = paragraphService;

            DeletePictureStyleCommand = new AsyncRelayCommand(DeletePictureStyle, CanDelete);
        }

        public ICommand DeletePictureStyleCommand { get; }

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
    }
}
