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
    public class TemplateService : BaseDataService<Template>
    {
        public TemplateService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<bool> IsUnique(string name)
        {
            await using var context = await _factory.CreateDbContextAsync();

            Template template = await context.Templates.Where(x => x.Name == name).FirstOrDefaultAsync();

            if (template == null)
            {
                return true;
            }
            return false;
        }
    }
}
