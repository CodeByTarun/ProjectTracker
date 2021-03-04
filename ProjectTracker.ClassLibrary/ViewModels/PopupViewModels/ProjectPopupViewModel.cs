using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.Interfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public class ProjectPopupViewModel : AbstractPopupViewModel
    {
        #region Fields
        // Project Properties
        private Project _projectToEdit;

        private string _description;

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                RaisePropertyChangedEvent(nameof(Description));
            }
        }

        // Services
        private IDataService<Project> _projectService;
        #endregion

        #region Constructors
        public ProjectPopupViewModel()
        {
            IsVisible = false;
            CreateCommands();
            DialogTitle = "Create a Project";
            ButtonContent = "Create";
        }

        public ProjectPopupViewModel(IDataService<Project> projectService) : base()
        {
            _projectService = projectService;
        }
        #endregion

        #region Command Functions

        public void ShowCreateProjectPopup()
        {
            DialogTitle = "Create a Project";
            ButtonContent = "Create";

            IsVisible = true;

        }
        public void ShowEditProjectPopup(Project projectToEdit)
        {
            _isEdit = true;
            _projectToEdit = projectToEdit;

            Name = projectToEdit.Name;
            Description = projectToEdit.Description;

            DialogTitle = "Edit Project";
            ButtonContent = "Save";

            IsVisible = true;
        }

        protected override void ResetFields()
        {
            Name = "";
            Description = "";
            _projectToEdit = null;
            _isEdit = false;
        }

        protected async override void CreateItem()
        {
            Project project = new Project()
            {
                Name = Name,
                Description = Description,
                DateCreated = DateTime.Now
            };

            await _projectService.Create(project);
        }

        protected async override void EditItem()
        {
            _projectToEdit.Name = Name;
            _projectToEdit.Description = Description;

            await _projectService.Update(_projectToEdit.Id, _projectToEdit);
        }

        #endregion
    }
}
