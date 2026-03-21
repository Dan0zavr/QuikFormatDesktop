using Microsoft.EntityFrameworkCore;
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
        public TextService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<bool> IsUnique(string name)
        {
            await using var context = await _factory.CreateDbContextAsync();

            TextStyle textStyle = await context.TextStyles.Where(x => x.Name == name).FirstOrDefaultAsync();

            if (textStyle == null)
            {
                return true;
            }
            return false;
        }
    }
}
