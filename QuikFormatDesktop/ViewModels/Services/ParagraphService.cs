using Microsoft.EntityFrameworkCore;
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
        public ParagraphService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<bool> IsUnique(string name)
        {
            await using var context = await _factory.CreateDbContextAsync();

            ParagraphStyle paragraphStyle = await context.ParagraphStyles.Where(x => x.Name == name).FirstOrDefaultAsync();

            if (paragraphStyle == null)
            {
                return true;
            }
            return false;
        }
    }
}
