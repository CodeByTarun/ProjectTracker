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
        public DeletePopupViewModel DeletePopupViewModel { get; private set; }

        public MainViewModel(TabViewModel tabViewModel, HomeViewModel homeViewModel, ProjectPopupViewModel projectPopupViewModel, BoardPopupViewModel boardPopupViewModel,
            GroupPopupViewModel groupPopupViewModel, IssuePopupViewModel issuePopupViewModel, TagPopupViewModel tagPopupViewModel, DeletePopupViewModel deletePopupViewModel)
        {
            TabViewModel = tabViewModel;
            HomeViewModel = homeViewModel;

            ProjectPopupViewModel = projectPopupViewModel;
            BoardPopupViewModel = boardPopupViewModel;
            GroupPopupViewModel = groupPopupViewModel;
            IssuePopupViewModel = issuePopupViewModel;
            TagPopupViewModel = tagPopupViewModel;
            DeletePopupViewModel = deletePopupViewModel;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            HomeViewModel.ProjectListViewModel.UpdateTabsEvent += ProjectListViewModel_UpdateTabsEvent;
            TagPopupViewModel.AddTagEvent += TagPopupViewModel_AddTagEvent;
            TagPopupViewModel.EditOrDeleteTagEvent += TagPopupViewModel_EditOrDeleteTagEvent;
            TabViewModel.ProjectListChanged += TabViewModel_ProjectListChanged;
        }

        private void TabViewModel_ProjectListChanged(object sender, EventArgs e)
        {
            HomeViewModel.ProjectListViewModel.GetProjectList(null);
        }

        private void ProjectListViewModel_UpdateTabsEvent(object isEdit, EventArgs e)
        {
            if ((bool)isEdit == true)
            { 
                TabViewModel.CheckTabs(HomeViewModel.ProjectListViewModel.SelectedProject);
            } else
            {
                TabViewModel.RemoveTabIfOpen(HomeViewModel.ProjectListViewModel.SelectedProject);
            }
        }

        // For this just need to update the popups and tag filter (when created for kanban board, board list, and project list)
        private void TagPopupViewModel_AddTagEvent(object sender, EventArgs e)
        {
            UpdateTagsInPopups();
        }

        // For this need to update the above and update the board list, kanban board, and project list
        private void TagPopupViewModel_EditOrDeleteTagEvent(object sender, EventArgs e)
        {
            UpdateTagsInPopups();
            UpdateTagsInControls();
        }

        private void UpdateTagsInPopups()
        {
            ProjectPopupViewModel.GetTagList();
            BoardPopupViewModel.GetTagList();
            IssuePopupViewModel.GetTagList();
        }

        private void UpdateTagsInControls()
        {
            TabViewModel.UpdateTagsInAllTabs();
            HomeViewModel.ProjectListViewModel.GetProjectList(null);
        }
    }
}
