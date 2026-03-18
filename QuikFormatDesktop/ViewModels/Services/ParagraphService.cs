using QuikFormatDesktop.Database;
using QuikFormatDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuikFormatDesktop.ViewModels.Services
{
    public class ParagraphService : BaseDataService<ParagraphStyle>
    {
        public ParagraphService(QfDbContext context) : base(context) { }
    }
}
