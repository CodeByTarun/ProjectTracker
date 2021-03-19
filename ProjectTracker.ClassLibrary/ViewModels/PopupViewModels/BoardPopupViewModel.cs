using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.Interfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public class BoardPopupViewModel : TagExtensionAbstractPopupViewModel
    {
        // Extra Fields
        private string _description;
        private int _projectId;
        private Board _boardToEdit;

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

            if (boardToEdit.Tags != null)
            {
                foreach (Tag tag in boardToEdit.Tags)
                {
                    ItemTags.Add(tag);
                }
            }

            TagSearchText = "";

            DialogTitle = "Edit Project";
            ButtonContent = "Save";

            IsVisible = true;
        }

        protected async override void CreateItem()
        {
            Board board = new Board()
            {
                Name = Name,
                Description = Description,
                Tags = ItemTags,
                DateCreated = DateTime.Now,
                ProjectID = _projectId
            };

            await _boardDataService.Create(board);
        }
        protected async override void EditItem()
        {
            _boardToEdit.Name = Name;
            _boardToEdit.Description = Description;
            _boardToEdit.Tags = ItemTags;

            await _boardDataService.Update(_boardToEdit.Id , _boardToEdit);
        }

        protected override void ResetFields()
        {
            Name = "";
            Description = "";
            _boardToEdit = null;
            _projectId = 0;
            _isEdit = false;

            base.ResetFields();
        }
    }
}
