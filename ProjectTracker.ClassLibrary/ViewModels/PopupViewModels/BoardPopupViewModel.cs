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
    public class BoardPopupViewModel : AbstractPopupViewModel
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
        public BoardPopupViewModel()
        {
            IsVisible = false;
            CreateCommands();

            DialogTitle = "Create a Board";
            ButtonContent = "Create";
        }
        public BoardPopupViewModel(IBoardDataService boardDataService) : base()
        {
            this._boardDataService = boardDataService;
        }

        // Methods for showing Create and Edit Popups
        public void ShowCreateBoardPopup(Project project)
        {
            _projectId = project.Id;

            DialogTitle = "Create a Board";
            ButtonContent = "Create";

            IsVisible = true;
        }
        public void ShowEditBoardPopup(Board boardToEdit)
        {
            _isEdit = true;

            _boardToEdit = boardToEdit;
            _projectId = boardToEdit.ProjectID;

            Name = boardToEdit.Name;
            Description = boardToEdit.Description;

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
                DateCreated = DateTime.Now,
                ProjectID = _projectId
            };

            await _boardDataService.Create(board);
        }
        protected async override void EditItem()
        {
            _boardToEdit.Name = Name;
            _boardToEdit.Description = Description;

            await _boardDataService.Update(_boardToEdit.Id , _boardToEdit);
        }

        protected override void ResetFields()
        {
            Name = "";
            Description = "";
            _boardToEdit = null;
            _projectId = 0;
            _isEdit = false;
        }
    }
}
