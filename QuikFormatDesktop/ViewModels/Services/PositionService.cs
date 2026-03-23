using Microsoft.EntityFrameworkCore;
using QuikFormatDesktop.Database;
using QuikFormatDesktop.Exceptions;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuikFormatDesktop.ViewModels.Services
{
    public class PositionService : BaseDataService<Position>
    {
        public PositionService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<int> GetIdByType(PositionType positionType)
        {
            await using var context = await _factory.CreateDbContextAsync();

            Position position = await context.Positions.Where(x => x.Position1 == positionType.ToString().ToLower()).FirstOrDefaultAsync();
            if (position != null)
            {
                return position.Id;
            }
            else
            {
                throw new AlignmentNotFoundException();
            }
        }
    }
}
