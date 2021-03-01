using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class MainViewModel
    {
        // ViewModels
        public TabViewModel TabViewModel { get; private set; }

        // PopupViewModels
        public ProjectPopupViewModel ProjectPopupViewModel {get; private set;}
        public BoardPopupViewModel BoardPopupViewModel { get; private set; }
        public GroupPopupViewModel GroupPopupViewModel { get; private set; }
        public IssuePopupViewModel IssuePopupViewModel { get; private set; }

        public MainViewModel(TabViewModel tabViewModel, ProjectPopupViewModel projectPopupViewModel, BoardPopupViewModel boardPopupViewModel, 
            GroupPopupViewModel groupPopupViewModel, IssuePopupViewModel issuePopupViewModel)
        {
            TabViewModel = tabViewModel;
            ProjectPopupViewModel = projectPopupViewModel;
            BoardPopupViewModel = boardPopupViewModel;
            GroupPopupViewModel = groupPopupViewModel;
            IssuePopupViewModel = issuePopupViewModel;
        }
    }
}
