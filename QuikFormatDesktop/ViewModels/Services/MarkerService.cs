using Microsoft.EntityFrameworkCore;
using QuikFormatDesktop.Database;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace QuikFormatDesktop.ViewModels.Services
{
    public class MarkerService : BaseDataService<Marker>
    {
        public MarkerService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<List<Marker>> GetByType(MarkerTypeEnum markerType)
        {
            await using var context = await _factory.CreateDbContextAsync();

            List<Marker> markers = await context.Markers
                .Where(t => t.MarkerTypeNavigation.Type == markerType.ToString().ToLower()).ToListAsync();

            return markers;
        }
    }
}
