using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public class IssuePopupViewModel : AbstractPopupViewModel
    {
        private Group _group;
        private Issue _issueToEdit;

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

        private IIssueDataService _issueDataService;

        public IssuePopupViewModel() : base()
        {
            DialogTitle = "Create an Issue";
            ButtonContent = "Create";
        }

        public IssuePopupViewModel(IIssueDataService issueDataService) : base()
        {
            this._issueDataService = issueDataService;
        }

        public void ShowCreateIssuePopup(Group group)
        {
            _group = group;

            DialogTitle = "Create an Issue";
            ButtonContent = "Create";

            IsVisible = true;
        }
        public void ShowEditIssuePopup(Issue issueToEdit)
        {
            _isEdit = true;
            _issueToEdit = issueToEdit;

            Name = issueToEdit.Name;
            Description = issueToEdit.Description;

            DialogTitle = "Edit Issue";
            ButtonContent = "Save";

            IsVisible = true;
        }

        protected async override void CreateItem()
        {
            Issue issue = new Issue()
            {
                Name = Name,
                Description = Description,
                DateCreated = DateTime.Now,
                GroupID = _group.Id
            };

            await _issueDataService.Create(issue);
        }
        protected async override void EditItem()
        {
            _issueToEdit.Name = Name;
            _issueToEdit.Description = Description;

            await _issueDataService.Update(_issueToEdit.Id, _issueToEdit);
        }

        protected override void ResetFields()
        {
            this.Name = "";
            this.Description = "";
            _group = null;
            _issueToEdit = null;
            _isEdit = false;
        }
    }
}
