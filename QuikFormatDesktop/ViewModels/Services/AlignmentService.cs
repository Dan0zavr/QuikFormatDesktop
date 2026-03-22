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
    public class AlignmentService : BaseDataService<Alignment>
    {
        public AlignmentService(IDbContextFactory<QfDbContext> factory) : base(factory) { }

        public async Task<int> GetIdByType(AlignmentType alignmentType)
        {
            await using var context = await _factory.CreateDbContextAsync();

            Alignment alignment = await context.Alignments.Where(x => x.Alignment1 == alignmentType.ToString().ToLower()).FirstOrDefaultAsync();
            if (alignment != null)
            {
                return alignment.Id;
            }
            else
            {
                throw new AlignmentNotFoundException();
            }
        }
    }
}
