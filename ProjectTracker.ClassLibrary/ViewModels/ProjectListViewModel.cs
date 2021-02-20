using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.Abstracts;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class ProjectListViewModel : TabViewModel
    {
        private IEnumerable<Project> projectList;
        private string projectSearchText;
        private Project selectedProject;

        public IEnumerable<Project> ProjectList
        {
            get
            {
                return projectList;
            }
            set
            {
                projectList = value;
                RaisePropertyChangedEvent(nameof(ProjectList));
            }
        }
        public string ProjectSearchText
        {
            get
            {
                return projectSearchText;
            }
            set
            {
                projectSearchText = value;
                RaisePropertyChangedEvent(ProjectSearchText);
                FilterProjectList();
            }
        }
        public Project SelectedProject
        {
            get
            {
                return selectedProject;
            }
            set
            {
                selectedProject = value;
                RaisePropertyChangedEvent(nameof(SelectedProject));
            }
        }
        public ICommand EditProjectCommand { get; private set; }
        public ICommand RemoveProjectCommand { get; private set; }

        public ProjectListViewModel()
        {
            ProjectList = new ObservableCollection<Project>();
            DummyProjectList();
            CreateCommands();
        }
        public ProjectListViewModel(IDataService<Project> projectService)
        {
            /// TODO: This needs to be taken from the database
            ProjectList = new ObservableCollection<Project>();
            /// TODO: Remove this later on
            DummyProjectList();
            CreateCommands();
        }
        private void DummyProjectList()
        {
            Project project1 = new Project()
            {
                Name = "Project 1 is a really long project that requires a lot to think about",
                Description = "first project",
                Id = 1
            };

            Project project2 = new Project()
            {
                Name = "Project 2",
                Description = "second project"
            };

            Project project3 = new Project()
            {
                Name = "Project 3",
                Description = "third project"
            };

            List<Project> projects = new List<Project> { project1, project2, project3 };

            ProjectList = projects;

            RaisePropertyChangedEvent(nameof(ProjectList));
        }
        private void CreateCommands()
        {
            EditProjectCommand = new RelayCommand(EditProject);
            RemoveProjectCommand = new RelayCommand(RemoveProject);
        }

        /// TODO
        public void EditProject(object selectedProject)
        {

        }
        /// TODO
        public void RemoveProject(object selectedProject)
        {

        }

        private void FilterProjectList()
        {
            ProjectList = ProjectList.Where(i => i.Name.ToLower().Contains(ProjectSearchText.ToLower()) || i.Description.ToLower().Contains(ProjectSearchText.ToLower()));
        }
    }
}
