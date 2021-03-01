using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.Interfaces
{
    public interface IPopupViewModel
    {
        bool IsVisible { get; }
        string DialogTitle { get; }
        string ButtonContent { get; }
        ICommand ClosePopupCommand { get; }

        ICommand CreateOrEditCommand { get; }
        event EventHandler CreateOrEditEvent;
    }
}
