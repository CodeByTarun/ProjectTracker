using Microsoft.EntityFrameworkCore.Design;
using NUnit.Framework;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.Services;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Tests.Tests
{
    public class ProjectDataSevriceTests
    {

        private IDesignTimeDbContextFactory<ProjectTrackerDBContext> _contextFactory;
        private SQLTestSetup _sQLTestSetup;

        private IProjectDataService _projectDataService;
        private ITagDataService _tagDataService;

        [SetUp]
        public void Setup()
        {
            _contextFactory = new ProjectTrackerTestDBContextFactory();

            _sQLTestSetup = new SQLTestSetup(_contextFactory);
            _sQLTestSetup.ProjectTest();

            _projectDataService = new ProjectDataService(_contextFactory);
            _tagDataService = new TagDataService(_contextFactory);

            _sQLTestSetup.CreateTagsForTests();
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

        [Test]
        public async Task CreateProjectWithTags_IsTrue()
        {
            Project project = await CreateProjectWithTags();

            Project projectReturned = await _projectDataService.Create(project);

            Assert.IsTrue(projectReturned.Tags.Any(i => i.Id == 1));
            Assert.IsTrue(projectReturned.Tags.Any(i => i.Id == 2));
        }

        [Test]
        public async Task GetProjectWithTags_IsTrue()
        {
            Project project = await CreateProjectWithTags();

            await _projectDataService.Create(project);

            Project projectReturned = await _projectDataService.Get(4);

            Assert.AreEqual(projectReturned.Tags.Count(), 2);
        }

        [Test]
        public async Task UpdateProjectWithTags_IsTrue()
        {
            Project project = await CreateProjectWithTags();

            Project projectReturned = await _projectDataService.Create(project);

            Tag tag = await _tagDataService.Get(1);
            Tag tag2 = await _tagDataService.Get(3);
            ICollection<Tag> tags = new Collection<Tag>() { tag, tag2 };

            projectReturned.Tags = tags;

            Project projectEdited = await _projectDataService.Update(4, projectReturned);
            Assert.IsTrue(projectEdited.Tags.Any(t => t.Name == "Task"));
        }

        [Test]
        public async Task GetAllProjectsWithTags_IsTrue()
        {
            Project project = await CreateProjectWithTags();

            await _projectDataService.Create(project);

            IEnumerable<Project> projects = await _projectDataService.GetAll();

            Assert.AreEqual(projects.Last().Tags.Count(), 2);
        }

        [Test]
        public async Task DeleteProjectWithTags_IsTrue()
        {
            Project project = await CreateProjectWithTags();

            await _projectDataService.Create(project);

            await _projectDataService.Delete(4);

            IEnumerable<Project> projects = await _projectDataService.GetAll();

            Assert.AreEqual(projects.Count(), 3);
        }

        private async Task<Project> CreateProjectWithTags()
        {
            Tag tag = await _tagDataService.Get(1);
            Tag tag2 = await _tagDataService.Get(2);
            ICollection<Tag> tags = new Collection<Tag>() { tag, tag2 };

            Project project = new Project
            {
                Name = "Another project",
                Tags = tags
            };

            return project;
        }
    }
}
