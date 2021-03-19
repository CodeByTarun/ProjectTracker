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
    public class TagDataServiceTests
    {
        private IDesignTimeDbContextFactory<ProjectTrackerDBContext> _contextFactory;
        private SQLTestSetup _sQLTestSetup;

        private ITagDataService _tagDataService;
        private IProjectDataService _projectDataService;

        [SetUp]
        public void Setup()
        {
            _contextFactory = new ProjectTrackerTestDBContextFactory();

            _sQLTestSetup = new SQLTestSetup(_contextFactory);
            _sQLTestSetup.ProjectTest();

            _projectDataService = new ProjectDataService(_contextFactory);
            _tagDataService = new TagDataService(_contextFactory);
        }

        [TearDown]
        public void Cleanup()
        {
            _sQLTestSetup.Cleanup();
        }

        [Test] 
        public async Task CreateTag_Works()
        {
            Tag tag = new Tag
            {
                Name = "Bug",
                IsFontBlack = true
            };

            await _tagDataService.Create(tag);
            Tag tagCreated = await _tagDataService.Get(1);

            Assert.AreEqual("Bug", tagCreated.Name);
        }
    }
}
