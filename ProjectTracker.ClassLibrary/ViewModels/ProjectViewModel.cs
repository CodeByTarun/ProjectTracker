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
        private int _selectedIndex;

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
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                RaisePropertyChangedEvent(nameof(SelectedIndex));
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

            SelectedIndex = 0;
        }

        private void SubscribeToEvents()
        {
            ProjectOverviewViewModel.BoardListViewModel.OpenBoardEvent += BoardListViewModel_OpenBoardEvent;
            ProjectOverviewViewModel.BoardListViewModel.RefreshBoardEvent += BoardListViewModel_RefreshBoardEvent;
            ProjectIssueViewModel.RefreshBoardEvent += ProjectIssueViewModel_RefreshBoardEvent;
        }

        private void BoardListViewModel_OpenBoardEvent(object sender, System.EventArgs e)
        {

            ProjectIssueViewModel.SelectedBoard = ProjectIssueViewModel.BoardList.FirstOrDefault(b => b.Id == ProjectOverviewViewModel.BoardListViewModel.SelectedBoard.Id);
            SelectedIndex = 1;
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
        private void ProjectIssueViewModel_RefreshBoardEvent(object sender, System.EventArgs e)
        {
            ProjectOverviewViewModel.BoardListViewModel.GetBoardList();
        }
    }
}
