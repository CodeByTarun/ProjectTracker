using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class MainViewModel
    {
        // ViewModels
        public TabViewModel TabViewModel { get; private set; }
        public HomeViewModel HomeViewModel { get; private set; }

        // PopupViewModels
        public ProjectPopupViewModel ProjectPopupViewModel { get; private set; }
        public BoardPopupViewModel BoardPopupViewModel { get; private set; }
        public GroupPopupViewModel GroupPopupViewModel { get; private set; }
        public IssuePopupViewModel IssuePopupViewModel { get; private set; }
        public TagPopupViewModel TagPopupViewModel { get; private set; }

        public MainViewModel(TabViewModel tabViewModel, HomeViewModel homeViewModel, ProjectPopupViewModel projectPopupViewModel, BoardPopupViewModel boardPopupViewModel,
            GroupPopupViewModel groupPopupViewModel, IssuePopupViewModel issuePopupViewModel, TagPopupViewModel tagPopupViewModel)
        {
            TabViewModel = tabViewModel;
            HomeViewModel = homeViewModel;

            ProjectPopupViewModel = projectPopupViewModel;
            BoardPopupViewModel = boardPopupViewModel;
            GroupPopupViewModel = groupPopupViewModel;
            IssuePopupViewModel = issuePopupViewModel;
            TagPopupViewModel = tagPopupViewModel;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            HomeViewModel.ProjectListViewModel.UpdateTabsEvent += ProjectListViewModel_UpdateTabsEvent;
        }

        // Need to check if the project edited or deleted is in the tab list
        private void ProjectListViewModel_UpdateTabsEvent(object sender, EventArgs e)
        {
            TabViewModel.CheckTabs(HomeViewModel.ProjectListViewModel.SelectedProject);
        }
    }
}
