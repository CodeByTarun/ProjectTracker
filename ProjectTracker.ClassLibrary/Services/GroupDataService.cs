using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.ClassLibrary.Services
{
    public class GroupDataService : GenericDataService<Group>, IGroupDataService
    {
        public GroupDataService(IDesignTimeDbContextFactory<ProjectTrackerDBContext> contextFactory) : base(contextFactory)
        {
        }

        public override async Task<Group> Create(Group entity)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                var nextT = await context.Set<Group>().FirstOrDefaultAsync((g) => g.BoardID == entity.BoardID && g.NextID == 0);
                entity.NextID = 0;

                var createdResult = await context.Set<Group>().AddAsync(entity);
                await context.SaveChangesAsync();

                if (nextT != null)
                {
                    nextT.NextID = createdResult.Entity.Id;
                    await context.SaveChangesAsync();
                }

                return createdResult.Entity;
            }
        }
        public override async Task<Group> Update(int id, Group entity)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                Group group = await context.Set<Group>().FirstOrDefaultAsync(g => g.Id == entity.Id);
                group.Name = entity.Name;
                context.Set<Group>().Update(group);
                await context.SaveChangesAsync();

                return entity;
            }
        }
        public override async Task<bool> Delete(int id)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                var entity = await context.Set<Group>().FirstOrDefaultAsync((e) => e.Id == id);
                var entityToChange = await context.Set<Group>().FirstOrDefaultAsync((e) => e.NextID == id);

                if (entityToChange != null)
                {
                    entityToChange.NextID = entity.NextID;
                }

                context.Set<Group>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> Move(Group entityMoving, Group entityBefore, Group entityAfter)
        {
            await UpdateEntityBeforeEntityMovingsOriginalLocation(entityMoving);

            await UpdateEntityBeforeWhereEntityIsMoving(entityMoving, entityBefore);

            await UpdateEntityAfterWhereEntityIsMoving(entityMoving, entityAfter);

            return true;
        }

        public async Task<bool> UpdateEntityBeforeEntityMovingsOriginalLocation(Group entityMovingOld)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                var entityMoving = await context.Set<Group>().FirstOrDefaultAsync((e) => e.Id == entityMovingOld.Id);
                var entityBeforeOriginalLocation = await context.Set<Group>().FirstOrDefaultAsync((e) => e.NextID == entityMovingOld.Id);

                if (entityBeforeOriginalLocation != null)
                {
                    entityBeforeOriginalLocation.NextID = entityMoving.NextID;
                    await context.SaveChangesAsync();
                }

                return true;
            }
        }
        public async Task<bool> UpdateEntityBeforeWhereEntityIsMoving(Group entityMovingOld, Group entityBeforeOld)
        {
            if (entityBeforeOld != null)
            {
                using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
                {
                    var entityMoving = await context.Set<Group>().FirstOrDefaultAsync((e) => e.Id == entityMovingOld.Id);
                    var entityBefore = await context.Set<Group>().FirstOrDefaultAsync((e) => e.Id == entityBeforeOld.Id);

                    entityBefore.NextID = entityMoving.Id;
                    await context.SaveChangesAsync();
                }
            }
            return true;
        }
        public async Task<bool> UpdateEntityAfterWhereEntityIsMoving(Group entityMovingOld, Group entityAfterOld)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                var entityMoving = await context.Set<Group>().FirstOrDefaultAsync((e) => e.Id == entityMovingOld.Id);

                if (entityAfterOld != null)
                {
                    var entityAfter = await context.Set<Group>().FirstOrDefaultAsync((e) => e.Id == entityAfterOld.Id);
                    entityMoving.NextID = entityAfter.Id;

                    await context.SaveChangesAsync();
                }
                else
                {
                    entityMoving.NextID = 0;
                    await context.SaveChangesAsync();
                }

                return true;
            }
        }
    }
}
