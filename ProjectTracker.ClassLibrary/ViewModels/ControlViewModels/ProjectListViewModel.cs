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
        private DeletePopupViewModel _deletePopupViewModel;

        private ProjectViewModelFactory _projectViewModelFactory;

        private IEnumerable<Project> projectList;
        private string projectSearchText;
        private Project selectedProject;

        private string _selectedStatus;
        public string SelectedStatus
        {
            get
            {
                return _selectedStatus;
            }
            set
            {
                _selectedStatus = value;
                RaisePropertyChangedEvent(nameof(SelectedStatus));
            }
        }

        private IEnumerable<Tag> _tagList;
        public IEnumerable<Tag> TagList
        {
            get
            {
                return _tagList;
            }
            set
            {
                _tagList = value;
                RaisePropertyChangedEvent(nameof(TagList));
            }
        }

        private Tag _selectedTag;
        public Tag SelectedTag
        {
            get
            {
                return _selectedTag;
            }
            set
            {
                _selectedTag = value;
                RaisePropertyChangedEvent(nameof(SelectedTag));
            }
        }

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
        public ICommand StatusFilterCommand { get; private set; }
        public ICommand TagFilterCommand { get; private set; }

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
        public ProjectListViewModel(TabViewModel tabViewModel, ProjectPopupViewModel projectPopupViewModel ,ProjectViewModelFactory projectViewModelFactory, 
            IProjectDataService projectDataService, DeletePopupViewModel deletePopupViewModel)
        {
            this._tabViewModel = tabViewModel;
            this._projectPopupViewModel = projectPopupViewModel;
            this._projectViewModelFactory = projectViewModelFactory;
            this._projectDataService = projectDataService;
            this._deletePopupViewModel = deletePopupViewModel;

            GetProjectList(null);
            CreateCommands();
        }

        #endregion

        public async void GetProjectList(object na)
        {
            ProjectList = await _projectDataService.GetAll();

            GetTagList();

            FilterProjectList();
        }

        private void GetTagList()
        {
            if (ProjectList != null)
            {
                Tag selectedTag = SelectedTag;

                TagList = ProjectList.SelectMany(p => p.Tags).Distinct().OrderBy(t => t.Name).ToList();

                if(selectedTag != null)
                {
                    SelectedTag = TagList.FirstOrDefault(t => t.Id == selectedTag.Id);
                }

                if (SelectedTag != null)
                {
                    if (!TagList.Any(t => t.Id == SelectedTag.Id))
                    {
                        SelectedTag = null;
                    }
                }
            }
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
            OpenProjectCommand = new RelayCommand(OpenProject, CanOpenProject);
            CreateProjectCommand = new RelayCommand(CreateProject);
            EditProjectCommand = new RelayCommand(EditProject);
            RemoveProjectCommand = new RelayCommand(ShowDeleteDialog);
            RefreshProjectListCommand = new RelayCommand(GetProjectList);
            StatusFilterCommand = new RelayCommand(StatusFilter);
            TagFilterCommand = new RelayCommand(TagFilter);
        }

        private bool CanOpenProject(object obj)
        {
            if (_tabViewModel.Tabs.Count <= 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }  

        public void OpenProject(object selectedProject)
        {
            if (selectedProject != null)
            {
                _tabViewModel.AddTab((Project)selectedProject);
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
            UpdateTabsEvent?.Invoke(true, EventArgs.Empty);

            GetProjectList(null);

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

        public void ShowDeleteDialog(object selectedProject)
        {
            if (selectedProject != null)
            {
                _deletePopupViewModel.ShowDeleteDialog(SelectedProject.Name);
                SubscribeToDeleteDialogEvents();
            }
        }

        private void SubscribeToDeleteDialogEvents()
        {
            _deletePopupViewModel.DeletedEvent += _deletePopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent += _deletePopupViewModel_CanceledEvent;
        }

        private void UnsubscribeToDeleteDialogEvents()
        {
            _deletePopupViewModel.DeletedEvent -= _deletePopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent -= _deletePopupViewModel_CanceledEvent;
        }

        private void _deletePopupViewModel_CanceledEvent(object sender, EventArgs e)
        {
            UnsubscribeToDeleteDialogEvents();
        }

        private async void _deletePopupViewModel_DeletedEvent(object sender, EventArgs e)
        {
            await _projectDataService.Delete(SelectedProject.Id);
            UpdateTabsEvent?.Invoke(false, EventArgs.Empty);
            GetProjectList(null);

            UnsubscribeToDeleteDialogEvents();
        }

        #endregion

        #region Filter Functions

        private void FilterProjectList()
        {
            if (ProjectList != null)
            {
                ProjectList = ProjectList
                                .Where(i => SelectedStatus == null || i.Status.Equals(SelectedStatus))
                                .Where(i => SelectedTag == null || i.Tags.Any(i => i.Id == SelectedTag.Id))
                                .Where(i => ProjectSearchText == null || (i.Name.ToLower().Contains(ProjectSearchText.ToLower()) || i.Description.ToLower().Contains(ProjectSearchText.ToLower())));
            }
        }

        private void TagFilter(object selectedTag)
        {
            FilterProjectList();
        }

        private void StatusFilter(object selectedStatus)
        {
            FilterProjectList();
        }

        #endregion
    }
}
