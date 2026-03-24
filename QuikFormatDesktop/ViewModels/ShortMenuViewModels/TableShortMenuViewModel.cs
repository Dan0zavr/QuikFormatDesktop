using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Commands;
using QuikFormatDesktop.ViewModels.Enums;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Input;

namespace QuikFormatDesktop.ViewModels.ShortMenuViewModels
{
    public class TableShortMenuViewModel : ViewModelBase
    {
        private readonly TableStyle _tableStyle;
        private readonly TableService _tableService;
        private readonly AlignmentService _alignmentService;
        private readonly TextService _textService;
        private readonly ParagraphService _paragraphService;

        public TableShortMenuViewModel(TableStyle tableStyle, TableService tableService, AlignmentService alignmentService, TextService textService, ParagraphService paragraphService)
        {
            _tableStyle = tableStyle;
            _tableService = tableService;
            _alignmentService = alignmentService;
            _textService = textService;
            _paragraphService = paragraphService;

            DeleteTableStyleCommand = new AsyncRelayCommand(DeleteTableStyle, CanDelete);
        }

        public ICommand DeleteTableStyleCommand { get; }

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
    }
}
