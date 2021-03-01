using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class ProjectOverviewViewModel
    {
        private Project _currentProject;
        public ProjectOverviewViewModel (Project currentProject)
        {
            this._currentProject = currentProject;
        }
    }
}
