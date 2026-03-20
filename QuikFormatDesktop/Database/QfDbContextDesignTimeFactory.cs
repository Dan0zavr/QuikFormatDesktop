using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QuikFormatDesktop.Database
{
    public class QfDbContextDesignTimeFactory : IDesignTimeDbContextFactory<QfDbContext>
    {
        public QfDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<QfDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlite(connectionString);

            return new QfDbContext(options.Options);
        }
    }
}
