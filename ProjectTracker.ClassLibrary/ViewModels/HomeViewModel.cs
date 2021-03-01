using System;
using System.Collections.Generic;
using System.Text;
using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class HomeViewModel
    {
        public ProjectListViewModel ProjectListViewModel { get; private set; }

        public HomeViewModel(ProjectListViewModel projectListViewModel)
        {
            this.ProjectListViewModel = projectListViewModel;
        }
    }
}
