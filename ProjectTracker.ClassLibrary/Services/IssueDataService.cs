using Microsoft.EntityFrameworkCore.Design;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectTracker.ClassLibrary.Services
{
    public class IssueDataService : GenericWithTagsDataService<Issue>, IIssueDataService
    {
        public IssueDataService(IDesignTimeDbContextFactory<ProjectTrackerDBContext> contextFactory) : base(contextFactory)
        {
        }

        public async override Task<Issue> Create(Issue entity)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                var nextT = await context.Set<Issue>().FirstOrDefaultAsync((i) => i.GroupID == entity.GroupID && i.NextID == 0);
                entity.NextID = 0;

                var createdResult = await context.Set<Issue>().AddAsync(entity);
                await context.SaveChangesAsync();

                if (nextT != null)
                {
                    nextT.NextID = createdResult.Entity.Id;
                    await context.SaveChangesAsync();
                }

                return createdResult.Entity;
            }
        }

        public async override Task<bool> Delete(int id)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                var entity = await context.Set<Issue>().FirstOrDefaultAsync((e) => e.Id == id);
                var entityToChange = await context.Set<Issue>().FirstOrDefaultAsync((e) => e.NextID == id);

                if (entityToChange != null)
                {
                    entityToChange.NextID = entity.NextID;
                }

                context.Set<Issue>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        /// TODO: This needs to be implemented
        public async Task<IEnumerable<string>> GetAllTags(int projectId)
        {
            return new List<string>();
        }

        public async Task Move(Issue issue, Issue issueBefore, Issue issueAfter, int groupMovedToId)
        {
            await UpdateIssueWithNextIDEqualToIssueMoving(issue);
            await UpdateIssueBeforeWhereIssueIsMoving(issue, issueBefore);
            await UpdateIssueMoving(issue, issueAfter, groupMovedToId);
        }

        private async Task UpdateIssueWithNextIDEqualToIssueMoving(Issue issue)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                var issueMoving = await context.Set<Issue>().FirstOrDefaultAsync((i) => i.Id == issue.Id);
                var issueBeforeOriginalLocation = await context.Set<Issue>().FirstOrDefaultAsync((i) => i.NextID == issue.Id);

                if (issueBeforeOriginalLocation != null)
                {
                    issueBeforeOriginalLocation.NextID = issueMoving.NextID;
                    await context.SaveChangesAsync();
                }
            }
        }
        private async Task UpdateIssueBeforeWhereIssueIsMoving(Issue issue, Issue issueBefore)
        {
            if (issueBefore != null)
            {
                using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
                {
                    var issueMoving = await context.Set<Issue>().FirstOrDefaultAsync((i) => i.Id == issue.Id);
                    var issueBeforeNewLocation = await context.Set<Issue>().FirstOrDefaultAsync((i) => i.Id == issueBefore.Id);

                    issueBeforeNewLocation.NextID = issueMoving.Id;
                    await context.SaveChangesAsync();
                }
            }
        }
        private async Task UpdateIssueMoving(Issue issue, Issue issueAFter, int groupMovedToId)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                var issueMoving = await context.Set<Issue>().FirstOrDefaultAsync((i) => i.Id == issue.Id);
                issueMoving.GroupID = groupMovedToId;

                if (issueAFter != null)
                {
                    var entityAfter = await context.Set<Issue>().FirstOrDefaultAsync((i) => i.Id == issueAFter.Id);
                    issueMoving.NextID = entityAfter.Id;
                }
                else
                {
                    issueMoving.NextID = 0;

                }
                await context.SaveChangesAsync();
            }
        }
    }
}
