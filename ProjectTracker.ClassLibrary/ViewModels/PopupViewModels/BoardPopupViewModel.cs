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
    public class BoardPopupViewModel : TagExtensionAbstractPopupViewModel
    {
        // Extra Fields
        private string _description;
        private string _status;
        private int _projectId;
        private Board _boardToEdit;
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
        IBoardDataService _boardDataService;

        // Constructor
        public BoardPopupViewModel(IBoardDataService boardDataService, ITagDataService tagDataService) : base(tagDataService)
        {
            this._boardDataService = boardDataService;
            InitialSetup();
        }

        #region Setup

        private void InitialSetup()
        {
            GetTagList();
            CreateCommands();
            Status = "Proposed";
        }

        #endregion 

        // Methods for showing Create and Edit Popups
        public void ShowCreateBoardPopup(Project project)
        {
            _projectId = project.Id;

            DialogTitle = "Create a Board";
            ButtonContent = "Create";

            TagSearchText = "";

            IsVisible = true;
        }
        public void ShowEditBoardPopup(Board boardToEdit)
        {
            _isEdit = true;

            _boardToEdit = boardToEdit;
            _projectId = boardToEdit.ProjectID;

            Name = boardToEdit.Name;
            Description = boardToEdit.Description;
            Status = boardToEdit.Status;
            DeadlineDate = boardToEdit.DeadlineDate;

            if (boardToEdit.Tags != null)
            {
                foreach (Tag tag in boardToEdit.Tags)
                {
                    ItemTags.Add(tag);
                }
            }

            TagSearchText = "";

            DialogTitle = "Edit Board";
            ButtonContent = "Save";

            IsVisible = true;
        }

        protected async override Task CreateItem()
        {
            Board board = new Board()
            {
                Name = Name,
                Description = Description,
                Status = Status,
                Tags = ItemTags,
                DateCreated = DateTime.Now,
                ProjectID = _projectId,
                DeadlineDate = DeadlineDate
            };

            await _boardDataService.Create(board);
        }
        protected async override Task EditItem()
        {
            _boardToEdit.Name = Name;
            _boardToEdit.Description = Description;
            _boardToEdit.Status = Status;
            _boardToEdit.Tags = ItemTags;
            _boardToEdit.DeadlineDate = DeadlineDate;

            await _boardDataService.Update(_boardToEdit.Id , _boardToEdit);
        }

        protected override void ResetFields()
        {
            Name = "";
            Description = "";
            Status = "Proposed";
            DeadlineDate = null;
            _boardToEdit = null;
            _projectId = 0;
            _isEdit = false;

            base.ResetFields();
        }
    }
}
