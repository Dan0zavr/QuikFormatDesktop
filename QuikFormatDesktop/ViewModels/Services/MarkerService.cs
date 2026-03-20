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
    public class MarkerService : BaseDataService<Marker>
    {
        public MarkerService(IDbContextFactory<QfDbContext> factory) : base(factory) { }
    }
}
