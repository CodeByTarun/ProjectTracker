using Microsoft.EntityFrameworkCore.Design;
using NUnit.Framework;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.Services;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Tests.Tests
{
    public class BoardDataServiceTests
    {
        private IDesignTimeDbContextFactory<ProjectTrackerDBContext> contextFactory;
        private SQLTestSetup sQLTestSetup;

        private IGroupDataService groupDataService;
        private IIssueDataService issueDataService;
        private IBoardDataService boardDataService;

        [SetUp]
        public void Setup()
        {
            contextFactory = new ProjectTrackerTestDBContextFactory();
            sQLTestSetup = new SQLTestSetup(contextFactory);

            groupDataService = new GroupDataService(contextFactory);
            issueDataService = new IssueDataService(contextFactory);
            boardDataService = new BoardDataService(contextFactory);

            sQLTestSetup.KanbanOrderTest();
        }

        [TearDown]
        public void Cleanup()
        {
            sQLTestSetup.Cleanup();
        }

        [Test]
        public async Task GroupList_FirstGroupIsNowLast_CorrectOrder()
        {
            ObservableCollection<Group> groups = await boardDataService.GetGroupsInBoard(1, contextFactory.CreateDbContext(null));

            await groupDataService.Move(groups[0], groups[2], null);

            ObservableCollection<Group> orderedGroups = await boardDataService.GetGroupsInBoard(1, contextFactory.CreateDbContext(null));

            Assert.AreEqual(1, orderedGroups[2].Id);
        }
        [Test]
        public async Task IssueList_FirstIssueIsNowLast_CorrectOrder()
        {
            sQLTestSetup.CreateIssues();

            ObservableCollection<Issue> issues = await boardDataService.GetIssuesInGroup(1, contextFactory.CreateDbContext(null));

            await issueDataService.Move(issues[0], issues[2], null, 1);

            ObservableCollection<Issue> orderedIssues = await boardDataService.GetIssuesInGroup(1, contextFactory.CreateDbContext(null));

            Assert.AreEqual(1, orderedIssues[2].Id);
        }

        [Test]
        public async Task CheckIfGroupByCollectionIsCorrect()
        {
            sQLTestSetup.CreateIssues();
            await CreateSecondIssueList();

            ObservableCollection<Issue> issues = await boardDataService.GetIssuesInBoard(1);

            Assert.AreEqual("First Issue", issues[3].Name);
            Assert.AreEqual("Third Issue", issues[5].Name);
        }

        public async Task CreateSecondIssueList()
        {
            Issue issue = new Issue
            {
                Name = "First Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 2
            };

            Issue issue2 = new Issue
            {
                Name = "Second Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 2
            };

            Issue issue3 = new Issue
            {
                Name = "Third Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 2
            };

            await issueDataService.Create(issue);
            await issueDataService.Create(issue2);
            await issueDataService.Create(issue3);
        }
    }
}
