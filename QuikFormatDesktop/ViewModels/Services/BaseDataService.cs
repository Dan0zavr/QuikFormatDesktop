using Microsoft.EntityFrameworkCore;
using QuikFormatDesktop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuikFormatDesktop.ViewModels.Services
{
    public abstract class BaseDataService<T> where T : class
    {
        protected readonly IDbContextFactory<QfDbContext> _factory;
        public event Action? StylesChanged;

        protected  BaseDataService(IDbContextFactory<QfDbContext> factory)
        {
            _factory = factory;
        }

        public async Task Add(T entity)
        {
            await using var context = await _factory.CreateDbContextAsync();

            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            StylesChanged?.Invoke();
        }

        public async Task Update(T entity)
        {
            await using var context = await _factory.CreateDbContextAsync();

            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            StylesChanged?.Invoke();
        }

        public async Task Delete(T entity)
        {
            await using var context = await _factory.CreateDbContextAsync();

            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
            StylesChanged?.Invoke();
        }

        public async Task<List<T>> GetAll()
        {
            await using var context = await _factory.CreateDbContextAsync();

            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            await using var context = await _factory.CreateDbContextAsync();

            return await context.Set<T>().FindAsync(id);
        }
    }
}
