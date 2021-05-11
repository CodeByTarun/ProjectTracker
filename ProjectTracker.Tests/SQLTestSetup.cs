using Microsoft.EntityFrameworkCore.Design;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.Services;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.Tests
{
    public class SQLTestSetup
    {
        private IDesignTimeDbContextFactory<ProjectTrackerDBContext> _contextFactory;

        private IProjectDataService _projectDataService;
        private IBoardDataService _boardDataService;
        private IGroupDataService _groupDataService;
        private IIssueDataService _issueDataService;
        private ITagDataService _tagDataService;

        public SQLTestSetup(IDesignTimeDbContextFactory<ProjectTrackerDBContext> contextFactory)
        {
            _contextFactory = contextFactory;

            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                context.Database.EnsureCreated();
            }

            _projectDataService = new ProjectDataService(_contextFactory);
            _boardDataService = new BoardDataService(_contextFactory);
            _groupDataService = new GroupDataService(_contextFactory);
            _issueDataService = new IssueDataService(_contextFactory);
            _tagDataService = new TagDataService(_contextFactory);
        }

        public void ProjectTest()
        {
            CreateProjects();
        }
        public void BoardTest()
        {
            CreateProjects();
            CreateProjectBoards();
        }
        public void KanbanOrderTest()
        {
            CreateProjects();
            CreateProjectBoards();
            CreateProjectBoardGroups();
        }
        public void IssueTest()
        {
            CreateProjects();
            CreateProjectBoards();
            CreateProjectBoardGroups();
            CreateIssues();
        }
        public async void CreateTagsForTests()
        {
            Tag tag1 = new Tag
            {
                Name = "Bug",
                IsFontBlack = true
            };
            Tag tag2 = new Tag
            {
                Name = "Gaming",
                IsFontBlack = true
            };
            Tag tag3 = new Tag
            {
                Name = "Task",
                IsFontBlack = true
            };

            await _tagDataService.Create(tag1);
            await _tagDataService.Create(tag2);
            await _tagDataService.Create(tag3);
        }

        private async void CreateProjects()
        {
            Project project1 = new Project
            {
                Name = "Project1",
                DateCreated = DateTime.Now,
                Status = "Open",
                Description = "First Semester"
            };
            Project project2 = new Project
            {
                Name = "Project2",
                DateCreated = DateTime.Now,
                Status = "Closed",
                Description = "Second project"
            };
            Project project3 = new Project
            {
                Name = "Project3",
                DateCreated = DateTime.Now,
                Status = "Completed",
                Description = "Third project"
            };

            await _projectDataService.Create(project1);
            await _projectDataService.Create(project2);
            await _projectDataService.Create(project3);
        }
        private async void CreateProjectBoards()
        {
            Board board1 = new Board()
            {
                Name = "Calculus",
                Description = "Calc 1 w/ Prof.Andrews",
                DateCreated = DateTime.Now,
                DeadlineDate = DateTime.Now.AddDays(300),
                ProjectID = 1
            };
            Board board2 = new Board()
            {
                Name = "Algebra",
                Description = "Alg 1 w/ Prof.Drew",
                DateCreated = DateTime.Now,
                DeadlineDate = DateTime.Now.AddDays(300),
                ProjectID = 1
            };
            Board board3 = new Board()
            {
                Name = "Biolody",
                Description = "Bio 1 w/ Prof.Smith",
                DateCreated = DateTime.Now,
                DeadlineDate = DateTime.Now.AddDays(300),
                ProjectID = 1
            };

            await _boardDataService.Create(board1);
            await _boardDataService.Create(board2);
            await _boardDataService.Create(board3);
        }
        private async void CreateProjectBoardGroups()
        {
            Group group1 = new Group()
            {
                BoardID = 1,
                Name = "To Do"
            };
            Group group2 = new Group()
            {
                BoardID = 1,
                Name = "In Progress"
            };
            Group group3 = new Group()
            {
                BoardID = 1,
                Name = "Completed"
            };

            await _groupDataService.Create(group1);
            await _groupDataService.Create(group2);
            await _groupDataService.Create(group3);
        }
        public async void CreateIssues()
        {
            Issue issue = new Issue
            {
                Name = "First Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 1
            };

            Issue issue2 = new Issue
            {
                Name = "Second Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 1
            };

            Issue issue3 = new Issue
            {
                Name = "Third Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 1
            };

            await _issueDataService.Create(issue);
            await _issueDataService.Create(issue2);
            await _issueDataService.Create(issue3);
        }

        public void Cleanup()
        {
            using (ProjectTrackerDBContext context = _contextFactory.CreateDbContext(null))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
