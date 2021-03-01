using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.Factories
{
    public class ProjectViewModelFactory
    {
        private ProjectItemsViewModelFactory _projectItemsViewModel;
        private IBoardDataService _boardDataService;

        public ProjectViewModelFactory(ProjectItemsViewModelFactory projectItemsViewModelFactory, IBoardDataService boardDataService)
        {
            this._projectItemsViewModel = projectItemsViewModelFactory;
            this._boardDataService = boardDataService;
        }

        public ProjectViewModel CreateProjectViewModel(Project project)
        {
            ProjectViewModel viewModel = new ProjectViewModel(_projectItemsViewModel.CreateProjectOverviewViewModel(project), _projectItemsViewModel.CreateProjectIssueViewModel(project), 
                _projectItemsViewModel.CreateProjectNotesViewModel(project), _boardDataService)
            {
                CurrentProject = project
            };

            return viewModel;
        }
    }
}
