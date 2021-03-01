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
            _itemId = 0;

            DialogTitle = "Create a Board";
            ButtonContent = "Create";

            IsVisible = true;
        }
        public void ShowEditBoardPopup(Board boardToEdit)
        {
            _itemId = boardToEdit.Id;
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
            Board board = new Board()
            {
                Name = Name,
                Description = Description,
                DateCreated = DateTime.Now,
                ProjectID = _projectId
            };

            await _boardDataService.Update(_itemId ,board);
        }

        protected override void ResetFields()
        {
            Name = "";
            Description = "";

            _projectId = 0;
        }
    }
}
