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
    }
}
