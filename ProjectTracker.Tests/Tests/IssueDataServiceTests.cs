using Microsoft.EntityFrameworkCore.Design;
using NUnit.Framework;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.Services;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Tests.Tests
{
    class IssueDataServiceTests
    {
        private IDesignTimeDbContextFactory<ProjectTrackerDBContext> _contextFactory;
        private SQLTestSetup _sQLTestSetup;

        private IIssueDataService  _issueDataService;

        [SetUp]
        public void Setup()
        {
            _contextFactory = new ProjectTrackerTestDBContextFactory();

            _sQLTestSetup = new SQLTestSetup(_contextFactory);
            _sQLTestSetup.KanbanOrderTest();

            _issueDataService = new IssueDataService(_contextFactory);
        }

        [TearDown]
        public void Cleanup()
        {
            _sQLTestSetup.Cleanup();
        }

        // Creating
        [Test]
        public async Task CreatedIssue_Exists_IsTrue()
        {
            Issue issue = new Issue
            {
                Name = "First Issue",
                Description = "Testing",
                Tag = "Bug",
                DateCreated = DateTime.Now,
                GroupID = 2
            };

            await _issueDataService.Create(issue);

            Issue issueCreated = await _issueDataService.Get(1);

            Assert.AreEqual(issue.Name, issueCreated.Name);
        }
        [Test]
        public async Task CreatedIssue_IsConnectedToLinkedList_IsTrue()
        {
            Issue issue = new Issue
            {
                Name = "First Issue",
                Description = "Testing",
                Tag = "Bug",
                DateCreated = DateTime.Now,
                GroupID = 2
            };

            Issue issue2 = new Issue
            {
                Name = "Second Issue",
                Description = "Testing",
                Tag = "Bug",
                DateCreated = DateTime.Now,
                GroupID = 2
            };

            await _issueDataService.Create(issue);
            await _issueDataService.Create(issue2);

            Issue issueCreated = await _issueDataService.Get(1);
            Issue issue2Created = await _issueDataService.Get(2);

            Assert.AreEqual(2, issueCreated.NextID);
            Assert.AreEqual(0, issue2Created.NextID);
        }

        // Deleting
        [Test]
        public async Task LastIssueDeleted_CorrectLinkedList_IsCorrect()
        {
            _sQLTestSetup.CreateIssues();

            await _issueDataService.Delete(3);

            Issue issue2 = await _issueDataService.Get(2);

            Assert.AreEqual(0, issue2.NextID);
        }
        [Test]
        public async Task MiddleIssueDeleted_CorrectLinkedList_IsTrue()
        {
            _sQLTestSetup.CreateIssues();

            await _issueDataService.Delete(2);

            Issue issue = await _issueDataService.Get(1);

            Assert.AreEqual(3, issue.NextID);
        }

        // Get Tags
        [Test]
        public async Task GetTags_NoIssues_NoErrorThrown()
        {
            _sQLTestSetup.CreateIssues();

            IEnumerable<string> tags = await _issueDataService.GetAllTags(1);

            Assert.IsTrue(tags.Contains("Bug"));
        }
        [Test]
        public async Task GetTags_MultipleIssues_NoDuplicates()
        {
            _sQLTestSetup.CreateIssues();

            IEnumerable<string> tags = await _issueDataService.GetAllTags(1);

            Assert.AreEqual(2, tags.Count());
        }

        // Moving to same group
        [Test]
        public async Task Issue_MoveLastToBeginningInSameGroup_Works()
        {
            _sQLTestSetup.CreateIssues();

            List<Issue> issues = await GetIssuesInList();

            await _issueDataService.Move(issues[2], null, issues[0], 1);

            List<Issue> issuesAfterMoving = await GetIssuesInList();

            Assert.AreEqual(2, issuesAfterMoving[0].NextID);
            Assert.AreEqual(0, issuesAfterMoving[1].NextID);
            Assert.AreEqual(1, issuesAfterMoving[2].NextID);
        }
        [Test]
        public async Task Issue_MoveFirstToEndInSameGroup_Works()
        {
            _sQLTestSetup.CreateIssues();

            List<Issue> issues = await GetIssuesInList();

            await _issueDataService.Move(issues[0], issues[2], null, 1);

            List<Issue> issuesAfterMoving = await GetIssuesInList();

            Assert.AreEqual(0, issuesAfterMoving[0].NextID);
            Assert.AreEqual(3, issuesAfterMoving[1].NextID);
            Assert.AreEqual(1, issuesAfterMoving[2].NextID);
        }
        [Test]
        public async Task Issue_MoveMiddleToEndInSameGroup_Works()
        {
            _sQLTestSetup.CreateIssues();

            List<Issue> issues = await GetIssuesInList();

            await _issueDataService.Move(issues[1], issues[2], null, 1);

            List<Issue> issuesAfterMoving = await GetIssuesInList();

            Assert.AreEqual(3, issuesAfterMoving[0].NextID);
            Assert.AreEqual(0, issuesAfterMoving[1].NextID);
            Assert.AreEqual(2, issuesAfterMoving[2].NextID);
        }
        [Test]
        public async Task Issue_MoveMiddleToBeginningInSameGroups_Works()
        {
            _sQLTestSetup.CreateIssues();

            List<Issue> issues = await GetIssuesInList();

            await _issueDataService.Move(issues[1], null, issues[0], 1);

            List<Issue> issuesAfterMoving = await GetIssuesInList();

            Assert.AreEqual(3, issuesAfterMoving[0].NextID);
            Assert.AreEqual(1, issuesAfterMoving[1].NextID);
            Assert.AreEqual(0, issuesAfterMoving[2].NextID);
        }

        // Moving to different group
        [Test]
        public async Task Issue_MovedToEmptyGroup_Works()
        {
            _sQLTestSetup.CreateIssues();

            List<Issue> issues = await GetIssuesInList();

            await _issueDataService.Move(issues[1], null, null, 2);

            List<Issue> issuesAfterMoving = await GetIssuesInList();

            Assert.AreEqual(3, issuesAfterMoving[0].NextID);
            Assert.AreEqual(0, issuesAfterMoving[1].NextID);
            Assert.AreEqual(0, issuesAfterMoving[2].NextID);

            Assert.AreEqual(2, issuesAfterMoving[1].GroupID);
        }
        [Test]
        public async Task Issue_MovedToEndOfAnotherGroup_Works()
        {
            _sQLTestSetup.CreateIssues();
            await CreateSecondIssueList();

            List<Issue> issues = await GetIssuesInList();
            List<Issue> issuesGroup2 = await GetSecondGroupIssuesInList();

            await _issueDataService.Move(issues[1], issuesGroup2[2], null, 2);

            Assert.AreEqual(3, (await _issueDataService.Get(1)).NextID);
            Assert.AreEqual(2, (await _issueDataService.Get(6)).NextID);
            Assert.AreEqual(0, (await _issueDataService.Get(2)).NextID);

            Assert.AreEqual(2, (await _issueDataService.Get(2)).GroupID);
        }
        [Test]
        public async Task Issue_MovedToBeginningOfAnotherGroup_Works()
        {
            _sQLTestSetup.CreateIssues();
            await CreateSecondIssueList();

            List<Issue> issues = await GetIssuesInList();
            List<Issue> issuesGroup2 = await GetSecondGroupIssuesInList();

            await _issueDataService.Move(issues[1], null, issuesGroup2[0], 2);

            Assert.AreEqual(3, (await _issueDataService.Get(1)).NextID);
            Assert.AreEqual(4, (await _issueDataService.Get(2)).NextID);

            Assert.AreEqual(2, (await _issueDataService.Get(2)).GroupID);
        }
        [Test]
        public async Task Issue_MovedToMiddleOfAnotherGroup_Works()
        {
            _sQLTestSetup.CreateIssues();
            await CreateSecondIssueList();

            List<Issue> issues = await GetIssuesInList();
            List<Issue> issuesGroup2 = await GetSecondGroupIssuesInList();

            await _issueDataService.Move(issues[1], issuesGroup2[0], issuesGroup2[1], 2);

            Assert.AreEqual(3, (await _issueDataService.Get(1)).NextID);
            Assert.AreEqual(2, (await _issueDataService.Get(4)).NextID);
            Assert.AreEqual(5, (await _issueDataService.Get(2)).NextID);

            Assert.AreEqual(2, (await _issueDataService.Get(2)).GroupID);
        }

        // Helpers
        public async Task<List<Issue>> GetIssuesInList()
        {
            List<Issue> issues = new List<Issue>
            {
                await _issueDataService.Get(1),
                await _issueDataService.Get(2),
                await _issueDataService.Get(3)
            };

            return issues;
        }
        public async Task<List<Issue>> GetSecondGroupIssuesInList()
        {
            List<Issue> issues = new List<Issue>
            {
                await _issueDataService.Get(4),
                await _issueDataService.Get(5),
                await _issueDataService.Get(6)
            };

            return issues;
        }
        public async Task CreateSecondIssueList()
        {
            Issue issue = new Issue
            {
                Name = "First Issue",
                Description = "Testing",
                Tag = "Bug",
                DateCreated = DateTime.Now,
                GroupID = 2
            };

            Issue issue2 = new Issue
            {
                Name = "Second Issue",
                Description = "Testing",
                Tag = "TODO",
                DateCreated = DateTime.Now,
                GroupID = 2
            };

            Issue issue3 = new Issue
            {
                Name = "Third Issue",
                Description = "Testing",
                Tag = "Bug",
                DateCreated = DateTime.Now,
                GroupID = 2
            };

            await _issueDataService.Create(issue);
            await _issueDataService.Create(issue2);
            await _issueDataService.Create(issue3);
        }
    }
}
