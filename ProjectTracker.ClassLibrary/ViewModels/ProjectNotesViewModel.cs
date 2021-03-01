using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class ProjectNotesViewModel
    {
        private Project _currentProject;

        public ProjectNotesViewModel(Project currentProject)
        {
            this._currentProject = currentProject;
        }
    }
}
