using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectTracker.ClassLibrary.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        protected readonly IDesignTimeDbContextFactory<ProjectTrackerDBContext> _contextFactory;

        public GenericDataService(IDesignTimeDbContextFactory<ProjectTrackerDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public virtual async Task<T> Create(T entity)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                var createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        public virtual async Task<bool> Delete(int id)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public virtual async Task<T> Get(int id)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                IEnumerable<T> entities = await context.Set<T>().ToListAsync();
                return entities;
            }
        }

        public virtual async Task<T> Update(int id, T entity)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                entity.Id = id;
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
        }
    }
}
