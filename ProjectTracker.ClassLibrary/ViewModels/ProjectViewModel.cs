using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.Model.Models;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class ProjectViewModel : ObservableObject
    {

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

        public ProjectViewModel()
        {

        }
    }
}
