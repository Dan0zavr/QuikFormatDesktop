using Microsoft.Extensions.Options;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Commands.ModalCommands;
using QuikFormatDesktop.ViewModels.Navigation;
using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class SystemTemplateViewModel : TemplateViewModel
    {
        private readonly IOptions<SystemStyles> _systemStyles;

        public SystemTemplateViewModel(TextService textService, ParagraphService paragraphService, TableService tableService, PictureService pictureService,
            NumberingService numberingService, FormulaService formulaService, GlobalStyleService globalService, TemplateService templateService, NavigationStore navigationStore, IOptions<SystemStyles> systemStyles) 
            : base(textService, paragraphService, tableService, pictureService, numberingService, formulaService, globalService, templateService, navigationStore)
        {
            _systemStyles = systemStyles;
        }

        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public string TextStyleName { get; set; }
        public string ParagraphStyleName { get; set; }
        public string TableStyleName { get; set; }
        public string PictureStyleName { get; set; }
        public string MarkedNumberingStyleName { get; set; }
        public string NumberedNumberingStyleName { get; set; }
        public string FormulaStyleName {  get; set; }

        public async Task InitializeAsync(Template template = null, bool isEdit = false)
        {
            if (template != null)
            {
                TemplateName = template.Name;
                TemplateDescription = template.Description;
                if (template.TextStyle != null) TextStyleName = _systemStyles.Value.TextStyles.FirstOrDefault(x => x.Id == template.TextStyle).Name;
                if (template.ParagraphStyle != null) ParagraphStyleName = _systemStyles.Value.ParagraphStyles.FirstOrDefault(x => x.Id == template.ParagraphStyle).Name;
                if (template.TableStyle != null) TableStyleName = _systemStyles.Value.TableStyles.FirstOrDefault(x => x.Id == template.TableStyle).Name;
                if (template.PictureStyle != null) PictureStyleName = _systemStyles.Value.PictureStyles.FirstOrDefault(x => x.Id == template.PictureStyle).Name;
                if (template.NumberedNumberingStyle != null) NumberedNumberingStyleName = _systemStyles.Value.NumberingStyles.FirstOrDefault(x => x.Id == template.NumberedNumberingStyle).Name;
                if (template.MarkedNumberingStyle != null) MarkedNumberingStyleName = _systemStyles.Value.NumberingStyles.FirstOrDefault(x => x.Id == template.MarkedNumberingStyle).Name;
                if (template.FormulaStyle != null) FormulaStyleName = _systemStyles.Value.FormulaStyles.FirstOrDefault(x => x.Id == template.FormulaStyle).Name;
            }
        }
    }
}
