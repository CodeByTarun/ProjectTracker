using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public class GroupPopupViewModel : AbstractPopupViewModel
    {
        private IGroupDataService _groupDataService;
        private Board _board;

        public GroupPopupViewModel() : base()
        {
            DialogTitle = "Create a Group";
            ButtonContent = "Create";
        }

        public GroupPopupViewModel(IGroupDataService groupDataService) : base()
        {
            this._groupDataService = groupDataService;
        }

        public void ShowCreateGroupPopup(Board board)
        {
            _itemId = 0;

            _board = board;

            DialogTitle = "Create a Group";
            ButtonContent = "Create";

            IsVisible = true;
        }

        public void ShowEditGroupPopup(Group groupToEdit, Board board)
        {
            _itemId = groupToEdit.Id;

            _board = board;

            Name = groupToEdit.Name;

            DialogTitle = "Edit Group";
            ButtonContent = "Save";

            IsVisible = true;
        }

        protected async override void CreateItem()
        {
            Group group = new Group()
            {
                Name = this.Name,
                BoardID = _board.Id
            };

            await _groupDataService.Create(group);
        }

        protected async override void EditItem()
        {
            Group group = new Group()
            {
                Name = this.Name,
                BoardID = _board.Id
            };

            await _groupDataService.Update(_itemId, group);
        }

        protected override void ResetFields()
        {
            Name = "";
            _board = null;
        }
    }
}
