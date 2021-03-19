using ProjectTracker.ClassLibrary.ServiceInterfaces;
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

        private IProjectDataService _projectDataService;

        public ProjectOverviewViewModel (Project currentProject, BoardListViewModel boardListViewModel, IProjectDataService projectDataService)
        {
            this._currentProject = currentProject;
            this.BoardListViewModel = boardListViewModel;
            this._projectDataService = projectDataService;
        }
        public ProjectOverviewViewModel()
        {

        }

        internal void UpdateTags()
        {
            UpdateCurrentProject();
            BoardListViewModel.GetBoardList();
        }

        private async void UpdateCurrentProject()
        {
            _currentProject = await _projectDataService.Get(_currentProject.Id);
        }
    }
}
