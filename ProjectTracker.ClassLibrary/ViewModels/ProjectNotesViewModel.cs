using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class ProjectNotesViewModel
    {
        private Project _currentProject;

        public ICommand CreateNoteCommand { get; private set; }
        public ICommand EditNoteCommand { get; private set; }
        public ICommand DeleteNoteCommand { get; private set; }

        public ProjectNotesViewModel(Project currentProject)
        {
            this._currentProject = currentProject;
        }

        #region Initial Setup
        private void InitialSetup()
        {
            CreateCommands();
        }
        private void CreateCommands()
        {

        }
        #endregion


    }
}
