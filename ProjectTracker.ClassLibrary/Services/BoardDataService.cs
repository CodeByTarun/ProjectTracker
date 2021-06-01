using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.ClassLibrary.Services
{
    public class BoardDataService : GenericWithTagsDataService<Board>, IBoardDataService
    {
        public BoardDataService(IDesignTimeDbContextFactory<ProjectTrackerDBContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<IEnumerable<Board>> GetAllInProject(int projectId)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                IEnumerable<Board> boards = await context.Set<Board>().Include(b => b.Tags).Where(b => b.ProjectID == projectId).ToListAsync();
                return boards;
            }
        }

        public async Task<Board> GetBoardWithInnerEntities(int boardId)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {

                Board entity = await context.Set<Board>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == boardId);

                ObservableCollection<Group> observableGroups = await GetGroupsInBoard(boardId, context);

                foreach (Group group in observableGroups)
                {
                    group.Issues = await GetIssuesInGroup(group.Id, context);
                }

                entity.Groups = observableGroups;

                return entity;
            }
        }
        public async Task<ObservableCollection<Issue>> GetIssuesInBoard(int boardId)
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                ObservableCollection<Group> groups = await GetGroupsInBoard(boardId, context);

                ObservableCollection<Issue> issues = new ObservableCollection<Issue>();

                foreach (Group group in groups)
                {
                    ObservableCollection<Issue> issueList = await GetIssuesInGroup(group.Id, context);

                    if (issueList.Count == 0)
                    {
                        Issue issue = new Issue
                        {
                            Id = 0,
                            Name = "",
                            Description = "",
                            GroupID = group.Id,
                            Group = group
                        };

                        issues.Add(issue);
                    }
                    else
                    {
                        foreach (Issue i in issueList)
                        {
                            issues.Add(i);
                        }
                    }
                }

                return issues;
            }
        }
        public async Task<ObservableCollection<Group>> GetGroupsInBoard(int boardId, ProjectTrackerDBContext context)
        {
            List<Group> groups = await context.Set<Group>().FromSqlRaw("WITH RECURSIVE my_tree as (SELECT * FROM Groups WHERE NextID = 0 AND BoardID = {0} " +
                    "UNION ALL SELECT g.Id, g.name, g.BoardID, g.NextID FROM Groups g join my_tree p on p.Id = g.NextID) SELECT * FROM my_tree", boardId).AsNoTracking().ToListAsync();

            ObservableCollection<Group> observableGroup = new ObservableCollection<Group>(Enumerable.Reverse(groups));

            return observableGroup;
        }
        public async Task<ObservableCollection<Issue>> GetIssuesInGroup(int groupId, ProjectTrackerDBContext context)
        {
            List<Issue> issues = await context.Set<Issue>().FromSqlRaw("WITH RECURSIVE my_tree as (SELECT * FROM Issues WHERE NextID = 0 AND GroupID = {0} " +
                    "UNION ALL SELECT g.Id, g.DateCreated, g.Deadlinedate, g.Description, g.GroupID, g.Name, g.NextID FROM Issues g join my_tree p on p.Id = g.NextID) SELECT * FROM my_tree", groupId).AsNoTracking().ToListAsync();

            ObservableCollection<Issue> observableIssues = new ObservableCollection<Issue>(issues);

            if (observableIssues != null)
            {
                foreach (Issue issue in observableIssues)
                {
                    issue.Tags = GetTagsInIssue(issue.Id, context);
                }
            }

            return observableIssues;
        }
        public ObservableCollection<Tag> GetTagsInIssue(int issuesId, ProjectTrackerDBContext context)
        {

            List<Tag> tags = (List<Tag>)context.Set<Issue>().AsNoTracking().Include(i => i.Tags).FirstOrDefault(i => i.Id == issuesId).Tags.ToList();

            if(tags != null)
            {
                return new ObservableCollection<Tag>(tags);
            } else
            {
                return null;
            }
        }
    }
}
