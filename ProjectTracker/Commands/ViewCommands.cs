using ProjectTracker.Helpers;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.Commands
{
    public static class ViewCommands
    {
        public static readonly ICommand OpenProjectInTabCommand = new RelayCommand(OpenProjectInTab);

        private static void OpenProjectInTab(object selectedProject)
        {
            TabControlHelper.AddTab((Project)selectedProject);
        }

    }
}
