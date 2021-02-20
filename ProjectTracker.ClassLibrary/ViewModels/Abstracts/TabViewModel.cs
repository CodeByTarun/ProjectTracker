using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels.Abstracts
{
    public abstract class TabViewModel : ObservableObject
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
    }
}
