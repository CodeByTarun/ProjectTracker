using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ProjectTracker.Helpers
{
    public class DialogService : IDialogService
    {
        public Window Owner { get; set; }

        public object CloseDialog()
        {
            throw new NotImplementedException();
        }

        public void ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}
