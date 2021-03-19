using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels;
using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.Factories
{
    public class ProjectItemsViewModelFactory
    {
        private IProjectDataService _projectDataService;
        private IBoardDataService _boardDataService;
        private IGroupDataService _groupDataService;
        private IIssueDataService _issueDataService;

        private BoardPopupViewModel _boardPopupViewModel;
        private GroupPopupViewModel _groupPopupViewModel;
        private IssuePopupViewModel _issuePopupViewModel;

        public ProjectItemsViewModelFactory(IProjectDataService projectDataService, IBoardDataService boardDataService, IGroupDataService groupDataService, IIssueDataService issueDataService, BoardPopupViewModel boardPopupViewModel, GroupPopupViewModel groupPopupViewModel, IssuePopupViewModel issuePopupViewModel)
        {
            _projectDataService = projectDataService;
            _boardDataService = boardDataService;
            _groupDataService = groupDataService;
            _issueDataService = issueDataService;
            _boardPopupViewModel = boardPopupViewModel;
            _groupPopupViewModel = groupPopupViewModel;
            _issuePopupViewModel = issuePopupViewModel;
        }

        public ProjectOverviewViewModel CreateProjectOverviewViewModel(Project currentProject)
        {
            BoardListViewModel boardListViewModel = CreateBoardListViewModel(currentProject);

            return new ProjectOverviewViewModel(currentProject, boardListViewModel, _projectDataService);
        }

        public ProjectIssueViewModel CreateProjectIssueViewModel(Project currentProject)
        {
            KanbanControlViewModel kanbanControlViewModel = CreateKanbanControlViewModel();

            return new ProjectIssueViewModel(currentProject, _boardDataService, _boardPopupViewModel, kanbanControlViewModel);
        }

        public ProjectNotesViewModel CreateProjectNotesViewModel(Project currentProject)
        {
            return new ProjectNotesViewModel(currentProject);
        }

        public KanbanControlViewModel CreateKanbanControlViewModel()
        {
            return new KanbanControlViewModel(_boardDataService, _groupDataService, _issueDataService, _groupPopupViewModel, _issuePopupViewModel);
        }

        public BoardListViewModel CreateBoardListViewModel(Project currentProject)
        {
            return new BoardListViewModel(currentProject, _boardDataService, _boardPopupViewModel);
        }
    }
}
