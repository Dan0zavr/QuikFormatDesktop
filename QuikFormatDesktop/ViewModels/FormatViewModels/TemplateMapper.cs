using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Wpf.Toolkit.Converters;

namespace QuikFormatDesktop.ViewModels.FormatViewModels
{
    public class TemplateMapper
    {
        private readonly TextService _textService;
        private readonly ParagraphService _paragraphService;
        private readonly PictureService _pictureService;
        private readonly TableService _tableService;
        private readonly NumberingService _numberingService; 
        private readonly FormulaService _formulaService;

        private readonly FontService _fontService;
        private readonly MarkerService _markerService;
        private readonly MarkerTypeService _markerTypeService;
        private readonly AlignmentService _alignmentService;
        private readonly PositionService _positionService;

        public TemplateMapper(TextService textService, ParagraphService paragraphService, PictureService pictureService,
                              TableService tableService, NumberingService numberingService, FormulaService formulaService, FontService fontService,
                              MarkerService markerService, MarkerTypeService markerTypeService, AlignmentService alignmentService, PositionService positionService)
        {
            _textService = textService;
            _paragraphService = paragraphService;
            _pictureService = pictureService;
            _tableService = tableService;
            _numberingService = numberingService;
            _formulaService = formulaService;
            _fontService = fontService;
            _markerService = markerService;
            _markerTypeService = markerTypeService;
            _alignmentService = alignmentService;
            _positionService = positionService;
        }

        public async Task<XMLParser.Styles.Template> MapToParser(Template template)
        {
            XMLParser.Styles.TextStyle textStyle = null;
            XMLParser.Styles.ParagraphStyle paragraphStyle = null; 
            XMLParser.Styles.NumberingElementStyle markedNumberingElementStyle = null;
            XMLParser.Styles.NumberingElementStyle numberedNumberingElementStyle = null;
            XMLParser.Styles.NumberingStyle numberingStyles = new XMLParser.Styles.NumberingStyle();
            XMLParser.Styles.TableStyle tableStyle = null;
            XMLParser.Styles.PictureStyle pictureStyle = null;
            XMLParser.Styles.FormulaStyle formulaStyle = null;

            if (template.TextStyle != null) textStyle = await MapTextStyle((int)template.TextStyle);
            if(template.ParagraphStyle != null) paragraphStyle = await MapParagraphStyle((int)template.ParagraphStyle);
            if(template.MarkedNumberingStyle != null) markedNumberingElementStyle = await MapNumberingStyle((int)template.MarkedNumberingStyle);
            if (template.NumberedNumberingStyle != null) numberedNumberingElementStyle = await MapNumberingStyle((int)template.NumberedNumberingStyle);
            numberingStyles.NumberedNumbering = numberedNumberingElementStyle;
            numberingStyles.MarkedNumbering = markedNumberingElementStyle;
            if(template.TableStyle != null) tableStyle = await MapTableStyle((int)template.TableStyle);
            if (template.PictureStyle != null) pictureStyle = await MapPictureStyle((int)template.PictureStyle);
            if(template.FormulaStyle != null) formulaStyle = await MapFormulaStyle((int) template.FormulaStyle);

            XMLParser.Styles.Template parserTemplate = new XMLParser.Styles.Template
            {
                TextStyle = textStyle,
                ParagraphStyle = paragraphStyle,
                NumberingStyle = numberingStyles,
                TableStyle = tableStyle,
                PictureStyle = pictureStyle,
                FormulaStyle = formulaStyle
            };

            return parserTemplate;
        }

        private async Task<XMLParser.Styles.TextStyle> MapTextStyle(int id)
        {
            TextStyle textStyle = await _textService.GetById(id);

            XMLParser.Styles.TextStyle parserStyle = new XMLParser.Styles.TextStyle
            {
                FontName = (await _fontService.GetById(textStyle.Font)).FontName,
                FontSize = textStyle.FontSize,
            };

            return parserStyle;
        }

        private async Task<XMLParser.Styles.ParagraphStyle> MapParagraphStyle(int id)
        {
            ParagraphStyle paragraphStyle = await _paragraphService.GetById(id);

            XMLParser.Styles.ParagraphStyle parserStyle = new XMLParser.Styles.ParagraphStyle
            {
                Alingnment = (await _alignmentService.GetById(paragraphStyle.Alignment)).Alignment1,
                FirstLineIndent = paragraphStyle.FirstLineIndent,
                LeftIndent = paragraphStyle.LeftIndent,
                RightIndent = paragraphStyle.RightIndent,
                IntervalInText = paragraphStyle.IntervalInText,
                BeforeInterval = paragraphStyle.BeforeInterval,
                AfterInterval = paragraphStyle.AfterInterval,
                ContextualSpacing = paragraphStyle.ContextualSpacing,
            };

            return parserStyle;
        }

        private async Task<XMLParser.Styles.NumberingElementStyle> MapNumberingStyle(int id)
        {
            NumberingStyle numberingStyle = await _numberingService.GetById(id);
            Marker marker = await _markerService.GetById(numberingStyle.Marker);

            XMLParser.Styles.NumberingElementStyle parserStyle = new XMLParser.Styles.NumberingElementStyle
            {
                Marker = marker.Marker1,
                NumberingType = (await _markerTypeService.GetById(marker.MarkerType)).Type
            };

            return parserStyle;
        }

        private async Task<XMLParser.Styles.TableStyle> MapTableStyle(int id)
        {
            TableStyle tableStyle = await _tableService.GetById(id);

            XMLParser.Styles.TextStyle parserTextStyle = await MapTextStyle(tableStyle.TextStyle);
            XMLParser.Styles.ParagraphStyle parserParagraphStyle = await MapParagraphStyle(tableStyle.ParagraphStyle);

            XMLParser.Styles.TableStyle parserStyle = new XMLParser.Styles.TableStyle
            {
                CellPadding = tableStyle.CellPadding,
                VerticalAlignment = (await _alignmentService.GetById(tableStyle.Alignment)).Alignment1,
                BorderThilness = tableStyle.BorderThikness,
                BorderColor = tableStyle.BorderColor,
                TextStyle = parserTextStyle,
                ParagraphStyle = parserParagraphStyle
            };

            return parserStyle;
        }

        private async Task<XMLParser.Styles.PictureStyle> MapPictureStyle(int id)
        {
            PictureStyle pictureStyle = await _pictureService.GetById(id);
            XMLParser.Styles.ParagraphStyle paragraphStyle = await MapParagraphStyle(pictureStyle.ParagraphStyle);

            XMLParser.Styles.PictureStyle parserStyle = new XMLParser.Styles.PictureStyle
            {
                AutoGenerateLable = pictureStyle.GenerateLabel,
                LabelValue = pictureStyle.LabelValue,
                ParagraphStyle = paragraphStyle,
                EmptyLineAround = pictureStyle.EmptyLineAround
            };
            return parserStyle;
        }

        private async Task<XMLParser.Styles.FormulaStyle> MapFormulaStyle(int id)
        {
            FormulaStyle formulaStyle = await _formulaService.GetById(id);
            Enum.TryParse( (await _positionService.GetById(formulaStyle.Position)).Position1, true, out XMLParser.Styles.AlignmentPreset alignment);

            string numerationFormat = null;

            if (formulaStyle.Numeration) numerationFormat = (await _markerService.GetById((int)formulaStyle.Marker)).Marker1;

            XMLParser.Styles.FormulaStyle parserStyle = new XMLParser.Styles.FormulaStyle
            {
                AlignmentPreset = alignment,
                Numeration = formulaStyle.Numeration,
                EmptyLineAround = formulaStyle.EmptyLineAround
            };

            if (numerationFormat != null)
            {
                parserStyle.NumerationFormat = numerationFormat;
            }
            return parserStyle;
        }
    }
}
