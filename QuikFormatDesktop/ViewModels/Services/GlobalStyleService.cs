using Microsoft.EntityFrameworkCore;
using QuikFormatDesktop.Database;
using System;
using System.Collections.Generic;
using System.Text;
using QuikFormatDesktop.Models;

namespace QuikFormatDesktop.ViewModels.Services
{
    public class GlobalStyleService : BaseDataService<GlobalStyle>
    {
        public GlobalStyleService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<bool> IsUnique(string name)
        {
            await using var context = await _factory.CreateDbContextAsync();

            GlobalStyle globalStyle = await context.GlobalStyles.Where(x => x.Name == name).FirstOrDefaultAsync();

            if (globalStyle == null)
            {
                return true;
            }
            return false;
        }
    }
}
