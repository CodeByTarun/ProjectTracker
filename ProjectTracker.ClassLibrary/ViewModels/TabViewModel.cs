using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model.Models;
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

        /// Design Time Constructor 
        public TabViewModel()
        {
            Tabs = new ObservableCollection<ProjectViewModel>();
            SelectedTab = null;

            DummyTabList();

            CreateCommands();
        }

        private void CreateCommands()
        {
            RemoveTabCommand = new RelayCommand(RemoveTab);
            GoToHomeCommand = new RelayCommand(GoToHome, CanGoToHome);
        }

        /// REMOVE THIS AFTER
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

        // Remove Tab Command
        private void RemoveTab(object tab)
        {
            int index = Tabs.IndexOf((ProjectViewModel)tab);
            ProjectViewModel tabSelected = SelectedTab;

            Tabs.Remove((ProjectViewModel)tab);

            ChangeTabOnRemove((ProjectViewModel)tab, tabSelected, index);                     
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
    }
}

