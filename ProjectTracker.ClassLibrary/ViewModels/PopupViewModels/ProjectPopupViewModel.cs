using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.Interfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public class ProjectPopupViewModel : TagExtensionAbstractPopupViewModel
    {
        #region Fields
        // Project Properties
        private Project _projectToEdit;

        private string _description;
        private string _status;
        private DateTime? _deadlineDate;

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
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                RaisePropertyChangedEvent(nameof(Status));
            }
        }
        public DateTime? DeadlineDate
        {
            get
            {
                return _deadlineDate;
            }
            set
            {
                _deadlineDate = value;
                RaisePropertyChangedEvent(nameof(DeadlineDate));
            }
        }

        // Services
        private IProjectDataService _projectService;
        #endregion

        #region Constructors

        public ProjectPopupViewModel(IProjectDataService projectService, ITagDataService tagDataService) : base(tagDataService)
        {
            _projectService = projectService;
            InitialSetup();
        }
        #endregion

        #region Setup

        private void InitialSetup()
        {
            PropertiesSetup();
            CreateCommands();
            Status = "Proposed";
        }

        private void PropertiesSetup()
        {
            IsVisible = false;
            DialogTitle = "Create a Project";
            ButtonContent = "Create";
        }

        #endregion

        #region Command Functions

        public void ShowCreateProjectPopup()
        {
            IsVisible = true;

            DialogTitle = "Create a Project";
            ButtonContent = "Create";

            TagSearchText = "";
        }
        public void ShowEditProjectPopup(Project projectToEdit)
        {
            IsVisible = true;

            DialogTitle = "Edit Project";
            ButtonContent = "Save";

            _isEdit = true;
            _projectToEdit = projectToEdit;

            Name = projectToEdit.Name;
            Description = projectToEdit.Description;
            Status = projectToEdit.Status;
            DeadlineDate = projectToEdit.DeadlineDate;

            if (projectToEdit.Tags != null)
            {
                foreach (Tag tag in projectToEdit.Tags)
                {
                    ItemTags.Add(tag);
                }
            }

            TagSearchText = "";
        }

        protected override void ResetFields()
        {
            Name = "";
            Description = "";
            Status = "Proposed";
            DeadlineDate = null;
            _projectToEdit = null;
            _isEdit = false;

            base.ResetFields();
        }

        protected async override Task CreateItem()
        {
            Project project = new Project()
            {
                Name = Name,
                Description = Description,
                Tags = ItemTags,
                Status = Status,
                DateCreated = DateTime.Now,
                DeadlineDate = DeadlineDate
            };

            await _projectService.Create(project);
        }

        protected async override Task EditItem()
        {
            _projectToEdit.Name = Name;
            _projectToEdit.Description = Description;
            _projectToEdit.Status = Status;
            _projectToEdit.Tags = ItemTags;
            _projectToEdit.DeadlineDate = DeadlineDate;

            await _projectService.Update(_projectToEdit.Id, _projectToEdit);
        }

        #endregion
    }
}
