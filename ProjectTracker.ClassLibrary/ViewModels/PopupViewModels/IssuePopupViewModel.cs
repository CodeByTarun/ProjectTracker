using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public class IssuePopupViewModel : TagExtensionAbstractPopupViewModel
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

        public IssuePopupViewModel(IIssueDataService issueDataService, ITagDataService tagDataService) : base(tagDataService)
        {
            this._issueDataService = issueDataService;
            InitialSetup();
        }

        #region Setup

        private void InitialSetup()
        {
            GetTagList();
        }

        #endregion 

        public void ShowCreateIssuePopup(Group group)
        {
            _group = group;

            DialogTitle = "Create an Issue";
            ButtonContent = "Create";

            IsVisible = true;

            TagSearchText = "";
        }
        public void ShowEditIssuePopup(Issue issueToEdit)
        {
            _isEdit = true;
            _issueToEdit = issueToEdit;

            Name = issueToEdit.Name;
            Description = issueToEdit.Description;

            if (issueToEdit.Tags != null)
            {
                foreach (Tag tag in issueToEdit.Tags)
                {
                    ItemTags.Add(tag);
                }
            }

            TagSearchText = "";

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
                Tags = ItemTags,
                DateCreated = DateTime.Now,
                GroupID = _group.Id
            };

            await _issueDataService.Create(issue);
        }
        protected async override void EditItem()
        {
            _issueToEdit.Name = Name;
            _issueToEdit.Description = Description;
            _issueToEdit.Tags = ItemTags;

            await _issueDataService.Update(_issueToEdit.Id, _issueToEdit);
        }

        protected override void ResetFields()
        {
            this.Name = "";
            this.Description = "";
            _group = null;
            _issueToEdit = null;
            _isEdit = false;

            base.ResetFields();
        }
    }
}
