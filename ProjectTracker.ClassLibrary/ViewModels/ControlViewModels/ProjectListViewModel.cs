using ProjectTracker.ClassLibrary.Factories;
using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.ControlViewModels
{
    public class ProjectListViewModel : ObservableObject
    {
        #region Fields

        private TabViewModel _tabViewModel;
        private ProjectPopupViewModel _projectPopupViewModel;

        private ProjectViewModelFactory _projectViewModelFactory;

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

        // Commands
        public ICommand OpenProjectCommand { get; private set; }
        public ICommand CreateProjectCommand { get; private set; }
        public ICommand EditProjectCommand { get; private set; }
        public ICommand RemoveProjectCommand { get; private set; }
        public ICommand RefreshProjectListCommand { get; private set; }

        // Services
        private IProjectDataService _projectDataService;

        public event EventHandler UpdateTabsEvent;

        #endregion

        #region Constructors

        public ProjectListViewModel()
        {
            ProjectList = new ObservableCollection<Project>();
            DummyProjectList();
            CreateCommands();
        }
        public ProjectListViewModel(TabViewModel tabViewModel, ProjectPopupViewModel projectPopupViewModel ,ProjectViewModelFactory projectViewModelFactory, IProjectDataService projectDataService)
        {
            this._tabViewModel = tabViewModel;
            this._projectPopupViewModel = projectPopupViewModel;
            this._projectViewModelFactory = projectViewModelFactory;
            this._projectDataService = projectDataService;

            GetProjectList(null);
            CreateCommands();
        }

        #endregion

        public async void GetProjectList(object na)
        {
            ProjectList = await _projectDataService.GetAll();
        }

        #region Commands

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

            ProjectList = new List<Project> { project1, project2, project3 };
        }
        private void CreateCommands()
        {
            OpenProjectCommand = new RelayCommand(OpenProject);
            CreateProjectCommand = new RelayCommand(CreateProject);
            EditProjectCommand = new RelayCommand(EditProject);
            RemoveProjectCommand = new RelayCommand(RemoveProject);
            RefreshProjectListCommand = new RelayCommand(GetProjectList);
        }
        public void OpenProject(object selectedProject)
        {
            ProjectViewModel vm = _tabViewModel.Tabs.FirstOrDefault(p => p.CurrentProject.Id == (selectedProject as Project).Id);
            int index = _tabViewModel.Tabs.IndexOf(vm);

            if (index >= 0)
            {
                _tabViewModel.SelectedTab = vm;
            } else
            {
                ProjectViewModel viewModel = _projectViewModelFactory.CreateProjectViewModel((Project)selectedProject);
                _tabViewModel.Tabs.Add(viewModel);
                _tabViewModel.SelectedTab = _tabViewModel.Tabs.LastOrDefault();
            }
        }
        public void CreateProject(object selectedProject)
        {
            SubscribeToEvents();

            _projectPopupViewModel.ShowCreateProjectPopup();
        }
        public void EditProject(object selectedProject)
        {
            SubscribeToEvents();

            _projectPopupViewModel.ShowEditProjectPopup(SelectedProject);
        }
        private void _projectPopupViewModel_CreateOrEditEvent(object sender, System.EventArgs e)
        {
            GetProjectList(null);

            UpdateTabsEvent?.Invoke(this, EventArgs.Empty);

            UnsubscribeToEvents();
        }
        private void _projectPopupViewModel_ClosePopupEvent(object sender, System.EventArgs e)
        {
            UnsubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _projectPopupViewModel.CreateOrEditEvent += _projectPopupViewModel_CreateOrEditEvent;
            _projectPopupViewModel.ClosePopupEvent += _projectPopupViewModel_ClosePopupEvent;
        }
        private void UnsubscribeToEvents()
        {
            _projectPopupViewModel.CreateOrEditEvent -= _projectPopupViewModel_CreateOrEditEvent;
            _projectPopupViewModel.ClosePopupEvent -= _projectPopupViewModel_ClosePopupEvent;
        }

        public async void RemoveProject(object selectedProject)
        {
            await _projectDataService.Delete(SelectedProject.Id);
            GetProjectList(null);
            UpdateTabsEvent?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        private void FilterProjectList()
        {
            ProjectList = ProjectList.Where(i => i.Name.ToLower().Contains(ProjectSearchText.ToLower()) || i.Description.ToLower().Contains(ProjectSearchText.ToLower()));
        }
    }
}
