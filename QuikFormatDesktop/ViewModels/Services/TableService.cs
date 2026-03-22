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
    public class TableService : BaseDataService<TableStyle>
    {
        public TableService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<bool> IsUnique(string name)
        {
            await using var context = await _factory.CreateDbContextAsync();

            TableStyle tableStyle = await context.TableStyles.Where(x => x.Name == name).FirstOrDefaultAsync();

            if (tableStyle == null)
            {
                return true;
            }
            return false;
        }
    }
}
