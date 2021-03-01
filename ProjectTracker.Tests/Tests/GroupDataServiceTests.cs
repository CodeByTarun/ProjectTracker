using Microsoft.EntityFrameworkCore.Design;
using NUnit.Framework;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.Services;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Tests.Tests
{
    public class GroupDataServiceTests
    {
        private IDesignTimeDbContextFactory<ProjectTrackerDBContext> _contextFactory;
        private SQLTestSetup _sQLTestSetup;

        private IGroupDataService _groupDataService;

        [SetUp]
        public void Setup()
        {
            _contextFactory = new ProjectTrackerTestDBContextFactory();

            _sQLTestSetup = new SQLTestSetup(_contextFactory);
            _sQLTestSetup.KanbanOrderTest();

            _groupDataService = new GroupDataService(_contextFactory);
        }

        [TearDown]
        public void Cleanup()
        {
            _sQLTestSetup.Cleanup();
        }


        // Creating
        [Test]
        public async Task CreatedGroupIsLast_IsTrue()
        {
            Group group = new Group()
            {
                BoardID = 1,
                Name = "On Hold"
            };

            await _groupDataService.Create(group);

            Group groupInDatabase = await _groupDataService.Get(4);
            Group beforeGroup = await _groupDataService.Get(3);

            Assert.AreEqual(0, groupInDatabase.NextID);
            Assert.AreEqual(4, beforeGroup.NextID);
        }

        // Deleting
        [Test]
        public async Task Delete_MiddleGroup_Deleted()
        {
            await _groupDataService.Delete(2);

            Group firstGroup = await _groupDataService.Get(1);

            Assert.AreEqual(3, firstGroup.NextID);
        }
        [Test]
        public async Task Delete_LastGroup_Deleted()
        {
            await _groupDataService.Delete(3);

            Group secondGroup = await _groupDataService.Get(2);

            Assert.AreEqual(0, secondGroup.NextID);
        }

        // Moving
        [Test]
        public async Task Move_FirstGroupToEnd_Moved()
        {
            List<Group> groups = await GetGroupsInList();

            await _groupDataService.Move(groups[0], groups[2], null);

            List<Group> groupsAfter = await GetGroupsInList();

            Assert.AreEqual(0, groupsAfter[0].NextID);
            Assert.AreEqual(3, groupsAfter[1].NextID);
            Assert.AreEqual(1, groupsAfter[2].NextID);
        }
        [Test]
        public async Task Move_FirstGroupToMiddle_Moved()
        {
            List<Group> groups = await GetGroupsInList();

            await _groupDataService.Move(groups[0], groups[1], groups[2]);

            List<Group> groupsAfter = await GetGroupsInList();

            Assert.AreEqual(3, groupsAfter[0].NextID);
            Assert.AreEqual(1, groupsAfter[1].NextID);
            Assert.AreEqual(0, groupsAfter[2].NextID);
        }

        [Test]
        public async Task Move_LastGroupToBeginning_Moved()
        {
            List<Group> groups = await GetGroupsInList();

            await _groupDataService.Move(groups[2], null, groups[0]);

            List<Group> groupsAfter = await GetGroupsInList();

            Assert.AreEqual(2, groupsAfter[0].NextID);
            Assert.AreEqual(0, groupsAfter[1].NextID);
            Assert.AreEqual(1, groupsAfter[2].NextID);
        }
        [Test]
        public async Task Move_LastGroupToMiddle_Moved()
        {
            List<Group> groups = await GetGroupsInList();

            await _groupDataService.Move(groups[2], groups[0], groups[1]);

            List<Group> groupsAfter = await GetGroupsInList();

            Assert.AreEqual(3, groupsAfter[0].NextID);
            Assert.AreEqual(0, groupsAfter[1].NextID);
            Assert.AreEqual(2, groupsAfter[2].NextID);
        }

        [Test]
        public async Task Move_MiddleGroupToBeginning_Moved()
        {
            List<Group> groups = await GetGroupsInList();

            await _groupDataService.Move(groups[1], null, groups[0]);

            List<Group> groupsAfter = await GetGroupsInList();

            Assert.AreEqual(3, groupsAfter[0].NextID);
            Assert.AreEqual(1, groupsAfter[1].NextID);
            Assert.AreEqual(0, groupsAfter[2].NextID);
        }
        [Test]
        public async Task Move_MiddleGroupToEnd_Moved()
        {
            List<Group> groups = await GetGroupsInList();

            await _groupDataService.Move(groups[1], groups[2], null);

            List<Group> groupsAfter = await GetGroupsInList();

            Assert.AreEqual(3, groupsAfter[0].NextID);
            Assert.AreEqual(0, groupsAfter[1].NextID);
            Assert.AreEqual(2, groupsAfter[2].NextID);
        }

        // Helpers
        public async Task<List<Group>> GetGroupsInList()
        {
            List<Group> groups = new List<Group>
            {
                await _groupDataService.Get(1),
                await _groupDataService.Get(2),
                await _groupDataService.Get(3)
            };

            return groups;
        }
    }
}
