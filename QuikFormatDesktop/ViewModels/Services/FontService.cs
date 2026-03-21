using Microsoft.EntityFrameworkCore;
using QuikFormatDesktop.Database;
using QuikFormatDesktop.Exceptions;
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
        public FontService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<int> GetIdByName(string fontName)
        {
            await using var context = await _factory.CreateDbContextAsync();

            Font font = await context.Fonts.Where(x => x.FontName == fontName).FirstOrDefaultAsync();
            if (font != null)
            {
                return font.Id;
            }
            else
            {
                throw new FontNotFoundException();
            }
        }
    }
}
