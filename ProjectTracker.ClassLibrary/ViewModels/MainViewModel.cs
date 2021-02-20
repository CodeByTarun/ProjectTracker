using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.Abstracts;
using ProjectTracker.Model.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class MainViewModel : ObservableObject
    {

        // Properties
        private ObservableCollection<Project> openProjects;
        private IEnumerable<Project> allProjects;
        private Project selectedProject;

        // Commands
        public ICommand AddTabCommand { get; private set; }
        public ICommand RemoveTabCommand { get; private set; }
        public ICommand GoToHomeCommand { get; private set; }

        public ObservableCollection<Project> OpenProjects
        {
            get
            {
                return openProjects;
            }
            set
            {
                openProjects = value;
                RaisePropertyChangedEvent(nameof(openProjects));
            }
        }
        public IEnumerable<Project> AllProjects
        {
            get
            {
                return allProjects;
            }
            set
            {
                allProjects = value;
                RaisePropertyChangedEvent(nameof(AllProjects));
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

        // Services
        private readonly IDataService<Project> projectDataService;

        /// Design Time Constructor 
        public MainViewModel()
        {
            OpenProjects = new ObservableCollection<Project>();
            SelectedProject = null;

            DummyProjectList();
        }
        public MainViewModel(IDataService<Project> projectDataService)
        {
            this.projectDataService = projectDataService;

            GetAllProjects();

            AllProjects = new ObservableCollection<Project>();
            OpenProjects = new ObservableCollection<Project>();
            SelectedProject = null;

            /// REMOVE THIS AFTER
            DummyProjectList();

            CreateCommands();
        }

        private void CreateCommands()
        {
            AddTabCommand = new RelayCommand(AddTab, CanCreateTab);
            RemoveTabCommand = new RelayCommand(RemoveTab);
            GoToHomeCommand = new RelayCommand(GoToHome, CanGoToHome);
        }
        private async void GetAllProjects()
        {
            AllProjects = await projectDataService.GetAll();
        }

        /// REMOVE THIS AFTER
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

            openProjects.Add(project1);
            openProjects.Add(project2);
            openProjects.Add(project3);

            RaisePropertyChangedEvent(nameof(OpenProjects));
        }

        private void AddTab(object openProjects)
        {
            Project project = new Project
            {
                Name = "Choose a Project",
                Id = 0
            };
            ((ObservableCollection<Project>)openProjects).Add(project);

            SelectedProject = OpenProjects.Last();
        }
        // AddTabCommand
        private bool CanCreateTab(object openProjects)
        {
            if (openProjects == null)
            {
                return true;
            }
            else
            {
                return (((ObservableCollection<Project>)openProjects).Count < 10);
            }
        }
        private void RemoveTab(object project)
        {
            int index = openProjects.IndexOf((Project)project);
            Project projectSelected = SelectedProject;
            OpenProjects.Remove((Project)project);

            ChangeTabOnRemove((Project)project, projectSelected, index);
        }
        private void ChangeTabOnRemove(Project projectRemoved, Project projectSelected, int index)
        {
            if (projectRemoved == projectSelected)
            {
                if (index < openProjects.Count())
                {
                    SelectedProject = openProjects.ElementAt(index);
                }
                else if ((index - 1) >= 0)
                {
                    SelectedProject = openProjects.ElementAt(index - 1);
                }
            }
        }

        // GoToHomeCommand
        private void GoToHome(object project)
        {
            SelectedProject = null;
        }
        private bool CanGoToHome(object project)
        {
            return project != null;
        }

        public void MoveTab(Project projectDragging, Project projectOver)
        {
            Project projectSelected = SelectedProject;

            int indexOfProjectOver = openProjects.IndexOf(projectOver);
            openProjects.Remove(projectDragging);
            openProjects.Insert(indexOfProjectOver, projectDragging);

            SelectedProject = projectSelected;
            RaisePropertyChangedEvent(nameof(OpenProjects));
        }
    }
}

