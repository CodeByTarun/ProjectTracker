using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class HomeViewModel
    {
        private TabViewModel _tabViewModel;

        public HomeViewModel(TabViewModel tabViewModel)
        {
            this._tabViewModel = tabViewModel;
        }
    }
}
