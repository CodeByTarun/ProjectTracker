using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class ProjectOverviewViewModel
    {
        private Project _currentProject;
        public BoardListViewModel BoardListViewModel { get; private set; }

        public ProjectOverviewViewModel (Project currentProject, BoardListViewModel boardListViewModel)
        {
            this._currentProject = currentProject;
            this.BoardListViewModel = boardListViewModel;
        }
        public ProjectOverviewViewModel()
        {

        }
    }
}
