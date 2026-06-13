using Microsoft.Extensions.Options;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup.Localizer;
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
        private readonly GlobalStyleService _globalService;

        private readonly FontService _fontService;
        private readonly MarkerService _markerService;
        private readonly MarkerTypeService _markerTypeService;
        private readonly AlignmentService _alignmentService;
        private readonly PositionService _positionService;

        private readonly IOptions<SystemStyles> _systemStyles;

        public TemplateMapper(TextService textService, ParagraphService paragraphService, PictureService pictureService,
                              TableService tableService, NumberingService numberingService, FormulaService formulaService, GlobalStyleService globalService, FontService fontService,
                              MarkerService markerService, MarkerTypeService markerTypeService, AlignmentService alignmentService, PositionService positionService, IOptions<SystemStyles> systemStyles)
        {
            _textService = textService;
            _paragraphService = paragraphService;
            _pictureService = pictureService;
            _tableService = tableService;
            _numberingService = numberingService;
            _formulaService = formulaService;
            _globalService = globalService;
            _fontService = fontService;
            _markerService = markerService;
            _markerTypeService = markerTypeService;
            _alignmentService = alignmentService;
            _positionService = positionService;
            _systemStyles = systemStyles;
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
            XMLParser.Styles.GlobalStyle globalStyle = null;

            if (template.TextStyle != null) textStyle = await MapTextStyle((int)template.TextStyle);
            if(template.ParagraphStyle != null) paragraphStyle = await MapParagraphStyle((int)template.ParagraphStyle);
            if(template.MarkedNumberingStyle != null) markedNumberingElementStyle = await MapNumberingStyle((int)template.MarkedNumberingStyle);
            if (template.NumberedNumberingStyle != null) numberedNumberingElementStyle = await MapNumberingStyle((int)template.NumberedNumberingStyle);
            numberingStyles.NumberedNumbering = numberedNumberingElementStyle;
            numberingStyles.MarkedNumbering = markedNumberingElementStyle;
            if(template.TableStyle != null) tableStyle = await MapTableStyle((int)template.TableStyle);
            if (template.PictureStyle != null) pictureStyle = await MapPictureStyle((int)template.PictureStyle);
            if(template.FormulaStyle != null) formulaStyle = await MapFormulaStyle((int) template.FormulaStyle);
            if(template.GlobalStyle != null) globalStyle = await MapGlobalStyle((int)template.GlobalStyle);

            XMLParser.Styles.Template parserTemplate = new XMLParser.Styles.Template
            {
                TextStyle = textStyle,
                ParagraphStyle = paragraphStyle,
                NumberingStyle = numberingStyles,
                TableStyle = tableStyle,
                PictureStyle = pictureStyle,
                FormulaStyle = formulaStyle,
                GlobalStyle = globalStyle
            };

            return parserTemplate;
        }

        private async Task<XMLParser.Styles.TextStyle> MapTextStyle(int id)
        {
            TextStyle textStyle = null;

            if (id < 0)
            {
                textStyle = _systemStyles.Value.TextStyles.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                textStyle = await _textService.GetById(id);
            }
            XMLParser.Styles.TextStyle parserStyle = new XMLParser.Styles.TextStyle
            {
                FontName = (await _fontService.GetById(textStyle.Font)).FontName,
                FontSize = textStyle.FontSize,
            };

            return parserStyle;
        }

        private async Task<XMLParser.Styles.ParagraphStyle> MapParagraphStyle(int id)
        {
            ParagraphStyle paragraphStyle = null;

            if (id < 0)
            {
                paragraphStyle = _systemStyles.Value.ParagraphStyles.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                paragraphStyle = await _paragraphService.GetById(id);
            }

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
            NumberingStyle numberingStyle = null;

            if (id < 0)
            {
                numberingStyle = _systemStyles.Value.NumberingStyles.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                numberingStyle = await _numberingService.GetById(id);
            }
            
            Marker marker = await _markerService.GetById(numberingStyle.Marker);
            string markerType = (await _markerTypeService.GetById(marker.MarkerType)).Type.ToString();

            string parserMarkerType = "";

            switch (markerType.ToLower())
            {
                case "marked":
                    parserMarkerType = "bullet";
                    break;
                case "numbered":
                    parserMarkerType = "decimal";
                    break;
                default:
                    break;
            }

            XMLParser.Styles.NumberingElementStyle parserStyle = new XMLParser.Styles.NumberingElementStyle
            {
                Marker = marker.Marker1,
                NumberingType = parserMarkerType
            };

            return parserStyle;
        }

        private async Task<XMLParser.Styles.TableStyle> MapTableStyle(int id)
        {
            TableStyle tableStyle = null;

            if (id < 0)
            {
                tableStyle = _systemStyles.Value.TableStyles.FirstOrDefault(x => x.Id == id);
            }
            else 
            {
                tableStyle = await _tableService.GetById(id);
            }

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
            PictureStyle pictureStyle = null;
            if (id < 0)
            {
                pictureStyle = _systemStyles.Value.PictureStyles.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                pictureStyle = await _pictureService.GetById(id);
            }
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
            FormulaStyle formulaStyle = null;

            if (id < 0)
            {
                formulaStyle = _systemStyles.Value.FormulaStyles.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                formulaStyle = await _formulaService.GetById(id);
            }

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

        private async Task<XMLParser.Styles.GlobalStyle> MapGlobalStyle(int id)
        {
            GlobalStyle globalStyle = null;

            if (id < 0)
            {
                globalStyle = _systemStyles.Value.GlobalStyles.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                globalStyle = await _globalService.GetById(id);
            }

            if (globalStyle == null)
                return null;

            var alignment = await _alignmentService.GetById(globalStyle.AlignmentId);

            XMLParser.Styles.GlobalStyle parserStyle = new XMLParser.Styles.GlobalStyle
            {
                LeftMargin = globalStyle.LeftMargin,
                RightMargin = globalStyle.RightMargin,
                TopMargin = globalStyle.TopMargin,
                BottomMargin = globalStyle.BottomMargin,
                SpecialColontitul = globalStyle.SpecialColontitul,
                LastNoNumberingPage = globalStyle.LastNoNumberingPage,
                Alignment = alignment.Alignment1
            };

            return parserStyle;
        }

    }
}
