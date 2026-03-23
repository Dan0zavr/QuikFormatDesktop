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

        public async Task<int> GetIdByName(string name)
        {
            await using var context = await _factory.CreateDbContextAsync();

            int id = await context.ParagraphStyles.Where(x => x.Name == name).Select(x => x.Id).FirstOrDefaultAsync();

            return id;
        }

        public async Task<List<string>> GetLikeNames(string name)
        {
            await using var context = await _factory.CreateDbContextAsync();

            List<string> result = await context.ParagraphStyles.Where(x => x.Name.Contains(name))
                                                               .Select(x => x.Name)
                                                               .ToListAsync();
            return result;
        }
    }
}
