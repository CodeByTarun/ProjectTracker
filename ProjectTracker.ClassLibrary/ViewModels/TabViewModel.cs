using ProjectTracker.ClassLibrary.Factories;
using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class TabViewModel : ObservableObject
    {

        // Properties
        private ProjectViewModel _selectedTab;

        // Commands
        public ICommand RemoveTabCommand { get; private set; }
        public ICommand GoToHomeCommand { get; private set; }
        public ICommand OpenTagsPopupCommand { get; private set; }

        public ObservableCollection<ProjectViewModel> Tabs { get; set; }
        public ProjectViewModel SelectedTab
        {
            get
            {
                return _selectedTab;
            }
            set
            {
                _selectedTab = value;
                RaisePropertyChangedEvent(nameof(SelectedTab));
            }
        }

        private TagPopupViewModel _tagPopupViewModel;
        private ProjectViewModelFactory _projectViewModelFactory;

        public event EventHandler ProjectListChanged;

        /// Design Time Constructor 
        public TabViewModel()
        {
            Tabs = new ObservableCollection<ProjectViewModel>();
            DummyTabList();
        }
        public TabViewModel(TagPopupViewModel tagPopupViewModel, ProjectViewModelFactory projectViewModelFactory)
        {
            _tagPopupViewModel = tagPopupViewModel;
            _projectViewModelFactory = projectViewModelFactory;

            Tabs = new ObservableCollection<ProjectViewModel>();
            SelectedTab = null;

            CreateCommands();
        }

        private void CreateCommands()
        {
            RemoveTabCommand = new RelayCommand(RemoveTab);
            GoToHomeCommand = new RelayCommand(GoToHome, CanGoToHome);
            OpenTagsPopupCommand = new RelayCommand(OpenTagPopup);
        }

        private void DummyTabList()
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

            ProjectViewModel p1 = new ProjectViewModel();
            ProjectViewModel p2 = new ProjectViewModel();
            ProjectViewModel p3 = new ProjectViewModel();

            p1.CurrentProject = project1;
            p2.CurrentProject = project2;
            p3.CurrentProject = project3;

            Tabs.Add(p1);
            Tabs.Add(p2);
            Tabs.Add(p3);
        }

        internal void UpdateTagsInAllTabs()
        {
            foreach (ProjectViewModel viewModel in Tabs)
            {
                viewModel.ProjectOverviewViewModel.UpdateTags();
                viewModel.ProjectIssueViewModel.UpdateTags();
            }
        }

        // Remove Tab Command
        public void AddTab(Project selectedProject)
        {
                ProjectViewModel vm = Tabs.FirstOrDefault(p => p.CurrentProject.Id == (selectedProject as Project).Id);
                int index = Tabs.IndexOf(vm);

                if (index >= 0)
                {
                    SelectedTab = vm;
                }
                else
                {
                    ProjectViewModel viewModel = _projectViewModelFactory.CreateProjectViewModel((Project)selectedProject);
                    SubscribeToProjectEvents(viewModel);
                    Tabs.Add(viewModel);
                    SelectedTab = Tabs.LastOrDefault();
                }
        }

        private void SubscribeToProjectEvents(ProjectViewModel viewModel)
        {
            viewModel.ProjectOverviewViewModel.BoardListViewModel.ProjectUpdatedEvent += BoardListViewModel_ProjectUpdatedEvent; ;
            viewModel.ProjectOverviewViewModel.BoardListViewModel.ProjectDeletedEvent += BoardListViewModel_ProjectDeletedEvent;
        }

        private void UnsubscribeToProjectEvents(ProjectViewModel viewModel)
        {
            viewModel.ProjectOverviewViewModel.BoardListViewModel.ProjectUpdatedEvent -= BoardListViewModel_ProjectUpdatedEvent;
            viewModel.ProjectOverviewViewModel.BoardListViewModel.ProjectDeletedEvent -= BoardListViewModel_ProjectDeletedEvent;
        }
        private void BoardListViewModel_ProjectUpdatedEvent(object sender, EventArgs e)
        {
            ProjectListChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BoardListViewModel_ProjectDeletedEvent(object viewModel, EventArgs e)
        {
            object projectViewModel = Tabs.First(t => t.CurrentProject.Id == ((BoardListViewModel)viewModel).CurrentProject.Id);
            RemoveTab(projectViewModel);
            ProjectListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveTab(object viewModel)
        {
            int index = Tabs.IndexOf((ProjectViewModel)viewModel);
            ProjectViewModel tabSelected = SelectedTab;

            ((ProjectViewModel)viewModel).CurrentProject = null;

            UnsubscribeToProjectEvents((ProjectViewModel)viewModel);

            Tabs.Remove((ProjectViewModel)viewModel);
            ChangeTabOnRemove((ProjectViewModel)viewModel, tabSelected, index);                     
        }
        private void ChangeTabOnRemove(ProjectViewModel tabRemoved, ProjectViewModel tabSelected, int index)
        {
            if (tabRemoved == tabSelected)
            {
                if (index < Tabs.Count())
                {
                    SelectedTab = Tabs.ElementAt(index);
                }
                else if ((index - 1) >= 0)
                {
                    SelectedTab = Tabs.ElementAt(index - 1);
                } else
                {
                    SelectedTab = null;
                }
            }
        }

        // GoToHomeCommand
        private void GoToHome(object tab)
        {
            SelectedTab = null;
        }
        private bool CanGoToHome(object tab)
        {
            return tab != null;
        }

        // OpenTagPopupCommand
        private void OpenTagPopup(object na)
        {
            _tagPopupViewModel.ShowTagPopup();
        }

        // Dragging
        public void MoveTab(ProjectViewModel tabDragging, ProjectViewModel tabOver)
        {
            ProjectViewModel tabSelected = SelectedTab;

            int indexOfTabOver = Tabs.IndexOf(tabOver);
            Tabs.Remove(tabDragging);
            Tabs.Insert(indexOfTabOver, tabDragging);

            SelectedTab = tabSelected;
            RaisePropertyChangedEvent(nameof(Tabs));
        }

        // Check if project is in Tabs collection
        internal async void CheckTabs(Project project)
        {
            foreach (ProjectViewModel viewModel in Tabs)
            {
                if (viewModel.CurrentProject.Id == project.Id)
                {
                    viewModel.CurrentProject = project;
                    await viewModel.ProjectOverviewViewModel.BoardListViewModel.UpdateProject();
                    break;
                }
            }
        }
        internal void RemoveTabIfOpen(Project project)
        {
            foreach (ProjectViewModel viewModel in Tabs)
            {
                if (viewModel.CurrentProject.Id == project.Id)
                {
                    Tabs.Remove(viewModel);
                    break;
                }
            }
        }
    }
}

