using ProjectTracker.ClassLibrary.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels.DialogViewModels
{
    public class ProjectDialogViewModel : IDialogViewModel
    {
        public int DialogWidth { get; set; }
        public int DialogHeight { get; set; }
        public string DialogTitle { get; set; }
    }
}
