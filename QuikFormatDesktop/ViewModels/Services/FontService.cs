using QuikFormatDesktop.Database;
using QuikFormatDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuikFormatDesktop.ViewModels.Services
{
    public class FontService : BaseDataService<Font>
    {
        public FontService(QfDbContext context) : base(context) { }
    }
}
