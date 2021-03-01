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
    public class ProjectDataSevriceTests
    {

        private IDesignTimeDbContextFactory<ProjectTrackerDBContext> _contextFactory;
        private SQLTestSetup _sQLTestSetup;

        private IDataService<Project> _projectDataService;

        [SetUp]
        public void Setup()
        {
            _contextFactory = new ProjectTrackerTestDBContextFactory();

            _sQLTestSetup = new SQLTestSetup(_contextFactory);
            _sQLTestSetup.ProjectTest();

            _projectDataService = new GenericDataService<Project>(_contextFactory);
        }

        [TearDown]
        public void Cleanup()
        {
            _sQLTestSetup.Cleanup();
        }

        [Test]
        public async Task ProjectsAreInDatabase_IsTrue()
        {
            Project project = await _projectDataService.Get(1);
            Console.WriteLine(project.Name);

            Assert.AreEqual("Project1", project.Name);
        }
    }
}
