using QuikFormatDesktop.Database;
using QuikFormatDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuikFormatDesktop.ViewModels.Services
{
    public class TextStyleService : BaseDataService<TextStyle>
    {
        public TextStyleService(QfDbContext context) : base(context) { }
    }
}
