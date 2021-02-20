using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels.Interfaces
{
    public interface IDialogViewModel
    {
        int DialogWidth { get; set; }
        int DialogHeight { get; set; }
        string DialogTitle { get; set; }
    }
}
