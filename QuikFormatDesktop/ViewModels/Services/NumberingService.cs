using Microsoft.EntityFrameworkCore;
using QuikFormatDesktop.Database;
using QuikFormatDesktop.Models;
using QuikFormatDesktop.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuikFormatDesktop.ViewModels.Services
{
    public class NumberingService : BaseDataService<NumberingStyle>
    {
        public NumberingService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<List<NumberingStyle>> GetStylesByType(MarkerTypeEnum markerType)
        {
            await using var context = await _factory.CreateDbContextAsync();

            List<NumberingStyle> styles = await context.NumberingStyles
                .Where(ns => ns.MarkerNavigation.MarkerTypeNavigation.Type == markerType.ToString().ToLower()).ToListAsync();
            return styles;
        }
    }
}
