using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ProjectTracker.Helpers
{
    public interface IDialogService
    {
        public Window Owner { get; set; }
        public void ShowDialog();
        public object CloseDialog();
    }
}
