using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.Database
{
    public class QfDbContextFactory
    {
        private readonly string _connectionString;

        public QfDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public QfDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<QfDbContext> options = new DbContextOptionsBuilder<QfDbContext>();
            options.UseSqlite(_connectionString);
            return new QfDbContext(options.Options);
        }
    }
}
