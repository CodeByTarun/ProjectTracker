﻿using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels;
using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.Factories
{
    public class ProjectItemsViewModelFactory
    {
        private IBoardDataService _boardDataService;
        private IGroupDataService _groupDataService;
        private IIssueDataService _issueDataService;

        private BoardPopupViewModel _boardPopupViewModel;
        private GroupPopupViewModel _groupPopupViewModel;
        private IssuePopupViewModel _issuePopupViewModel;

        public ProjectItemsViewModelFactory(IBoardDataService boardDataService, IGroupDataService groupDataService, IIssueDataService issueDataService, BoardPopupViewModel boardPopupViewModel, GroupPopupViewModel groupPopupViewModel, IssuePopupViewModel issuePopupViewModel)
        {
            _boardDataService = boardDataService;
            _groupDataService = groupDataService;
            _issueDataService = issueDataService;
            _boardPopupViewModel = boardPopupViewModel;
            _groupPopupViewModel = groupPopupViewModel;
            _issuePopupViewModel = issuePopupViewModel;
        }

        public ProjectOverviewViewModel CreateProjectOverviewViewModel(Project currentProject)
        {
            return new ProjectOverviewViewModel(currentProject);
        }

        public ProjectIssueViewModel CreateProjectIssueViewModel(Project currentProject)
        {
            KanbanControlViewModel kanbanControlViewModel = CreateKanbanControlViewModel();

            return new ProjectIssueViewModel(currentProject, _boardDataService, _boardPopupViewModel, kanbanControlViewModel);
        }

        public ProjectNotesViewModel CreateProjectNotesViewModel(Project currentProject)
        {
            return new ProjectNotesViewModel(currentProject);
        }

        public KanbanControlViewModel CreateKanbanControlViewModel()
        {
            return new KanbanControlViewModel(_boardDataService, _groupDataService, _issueDataService, _groupPopupViewModel, _issuePopupViewModel);
        }
    }
}