using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model.Models;
using System.Linq;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class ProjectViewModel : ObservableObject
    {

        public ProjectOverviewViewModel ProjectOverviewViewModel;
        public ProjectIssueViewModel ProjectIssueViewModel;
        public ProjectNotesViewModel ProjectNotesViewModel;

        private Project _currentProject;
        private bool _isShowingOverview;

        public Project CurrentProject
        {
            get
            {
                return _currentProject;
            }
            set
            {
                _currentProject = value;
                RaisePropertyChangedEvent(nameof(CurrentProject));
            }
        }
        public bool IsShowingOverview
        {
            get
            {
                return _isShowingOverview;
            }
            set
            {
                _isShowingOverview = value;
                RaisePropertyChangedEvent(nameof(IsShowingOverview));
            }
        }

        private IBoardDataService _boardDataService;

        public ProjectViewModel()
        {
            CurrentProject = new Project()
            {
                Name = "My First Project",
                Description = "This is the first of many projects that will be created"
            };
        }
        public ProjectViewModel(ProjectOverviewViewModel projectOverviewViewModel, ProjectIssueViewModel projectIssueViewModel, 
                                ProjectNotesViewModel projectNotesViewModel, IBoardDataService boardDataService)
        {
            this._boardDataService = boardDataService;
            this.ProjectOverviewViewModel = projectOverviewViewModel;
            this.ProjectIssueViewModel = projectIssueViewModel;
            this.ProjectNotesViewModel = projectNotesViewModel;

            SubscribeToEvents();

            IsShowingOverview = true;
        }
           
        private void SubscribeToEvents()
        {
            ProjectOverviewViewModel.BoardListViewModel.OpenBoardEvent += BoardListViewModel_OpenBoardEvent;
            ProjectOverviewViewModel.BoardListViewModel.RefreshBoardEvent += BoardListViewModel_RefreshBoardEvent;
            ProjectOverviewViewModel.BoardListViewModel.ProjectUpdatedEvent += BoardListViewModel_ProjectUpdatedEvent;
            ProjectIssueViewModel.RefreshBoardEvent += ProjectIssueViewModel_RefreshBoardEvent;
        }

        private void BoardListViewModel_OpenBoardEvent(object sender, System.EventArgs e)
        {
            if (ProjectOverviewViewModel.BoardListViewModel.SelectedBoard != null) 
            {
                ProjectIssueViewModel.SelectedBoard = ProjectIssueViewModel.BoardList.FirstOrDefault(b => b.Id == ProjectOverviewViewModel.BoardListViewModel.SelectedBoard.Id);
                IsShowingOverview = false;
            }
        }
        private void BoardListViewModel_RefreshBoardEvent(object sender, System.EventArgs e)
        {
            int boardId = 0;

            if (ProjectIssueViewModel.SelectedBoard != null)
            {
                boardId = ProjectIssueViewModel.SelectedBoard.Id;
            }

            ProjectIssueViewModel.GetBoardList();
            ProjectIssueViewModel.SelectedBoard = ProjectIssueViewModel.BoardList.FirstOrDefault(b => b.Id == boardId);
        }
        private void BoardListViewModel_ProjectUpdatedEvent(object sender, System.EventArgs e)
        {
            CurrentProject = ProjectOverviewViewModel.BoardListViewModel.CurrentProject;
        }
        private void ProjectIssueViewModel_RefreshBoardEvent(object sender, System.EventArgs e)
        {
            ProjectOverviewViewModel.BoardListViewModel.RefreshView();
        }
    }
}
