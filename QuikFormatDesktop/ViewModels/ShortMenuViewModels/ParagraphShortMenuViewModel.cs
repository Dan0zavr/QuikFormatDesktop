using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class ParagraphShortMenuViewModel : ViewModelBase
    {
        private readonly ParagraphStyle _paragraphStyle;
        private readonly ParagraphService _paragraphService;
        private readonly AlignmentService _alignmentService;

        public ParagraphShortMenuViewModel(ParagraphStyle paragraphStyle, ParagraphService paragraphService, AlignmentService alignmentService)
        {
            _paragraphStyle = paragraphStyle;
            _paragraphService = paragraphService;
            _alignmentService = alignmentService;
            DeleteParagraphStyleCommand = new AsyncRelayCommand(DeleteParagraphStyle, CanDelete);
        }

        public ICommand DeleteParagraphStyleCommand { get; }

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

        private bool CanDelete(object? parametr)
        {
            return true;
        }

        private async Task DeleteParagraphStyle(object? parametr)
        {
            await _paragraphService.Delete(_paragraphStyle);
        }
    }
}
