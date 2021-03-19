using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectTracker.Model;
using ProjectTracker.Model.Interfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.ClassLibrary.Services
{
    public class GenericWithTagsDataService<T> : GenericDataService<T> where T : DomainObjectWithTag
    {
        public GenericWithTagsDataService(IDesignTimeDbContextFactory<ProjectTrackerDBContext> contextFactory) : base(contextFactory)
        {
        }

        public async override Task<T> Create(T entity)
        {
            IEnumerable<Tag> tags = null;

            if (entity.Tags != null)
            {
                tags = entity.Tags;
            }

            entity.Tags = new Collection<Tag>();

            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                if (tags != null)
                {
                    foreach (Tag tag in tags)
                    {
                        Tag tagToAdd = context.Tags.FirstOrDefault(t => t.Id == tag.Id);
                        entity.Tags.Add(tagToAdd);
                    }
                }
                var createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        public async override Task<T> Update(int id, T entity)
        {
            entity.Id = id;

            IEnumerable<Tag> tags = null;

            if (entity.Tags != null)
            {
                tags = entity.Tags;
            }

            entity.Tags = new Collection<Tag>();

            RemoveTagsFromEntity(id);

            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                if (tags != null)
                {
                    foreach (Tag tag in tags)
                    {
                        Tag tagToAdd = context.Tags.FirstOrDefault(t => t.Id == tag.Id);
                        entity.Tags.Add(tagToAdd);
                    }
                }
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
        }

        public async override Task<T> Get(int id)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                T entity = await context.Set<T>().Include(t => t.Tags).FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async override Task<IEnumerable<T>> GetAll()
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                IEnumerable<T> entities = await context.Set<T>().Include(t => t.Tags).ToListAsync();
                return entities;
            }
        }

        public async override Task<bool> Delete(int id)
        {
            RemoveTagsFromEntity(id);

            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        private async void RemoveTagsFromEntity(int id)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                T entityTagRemoval = context.Set<T>().Include(t => t.Tags).FirstOrDefault(t => t.Id == id);

                if (entityTagRemoval.Tags != null)
                {
                    foreach (Tag tag in entityTagRemoval.Tags.ToList())
                    {
                        entityTagRemoval.Tags.Remove(tag);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
