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
    public class TextService : BaseDataService<TextStyle>
    {
        public TextService(QfDbContext context) : base(context) { }
    }
}
