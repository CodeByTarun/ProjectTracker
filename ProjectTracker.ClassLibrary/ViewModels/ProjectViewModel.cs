using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model.Models;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class ProjectViewModel : ObservableObject
    {

        public ProjectOverviewViewModel ProjectOverviewViewModel;
        public ProjectIssueViewModel ProjectIssueViewModel;
        public ProjectNotesViewModel ProjectNotesViewModel;

        private Project _currentProject;

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
        }
    }
}
