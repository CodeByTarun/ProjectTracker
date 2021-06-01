using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public class IssuePopupViewModel : TagExtensionAbstractPopupViewModel
    {
        private Group _group;
        private Issue _issueToEdit;
        private DateTime? _deadlineDate;

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
            CreateCommands();
        }

        #endregion 

        public void ShowCreateIssuePopup(Group group)
        {
            _group = group;

            DialogTitle = "Create a Task";
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
            DeadlineDate = issueToEdit.DeadlineDate;

            if (issueToEdit.Tags != null)
            {
                foreach (Tag tag in issueToEdit.Tags)
                {
                    ItemTags.Add(tag);
                }
            }

            TagSearchText = "";

            DialogTitle = "Edit Task";
            ButtonContent = "Save";

            IsVisible = true;
        }

        protected async override Task CreateItem()
        {
            Issue issue = new Issue()
            {
                Name = Name,
                Description = Description,
                Tags = ItemTags,
                DateCreated = DateTime.Now,
                GroupID = _group.Id,
                DeadlineDate = DeadlineDate
            };
            await _issueDataService.Create(issue);
        }
        protected async override Task EditItem()
        {
            _issueToEdit.Name = Name;
            _issueToEdit.Description = Description;
            _issueToEdit.Tags = ItemTags;
            _issueToEdit.DeadlineDate = DeadlineDate;

            await _issueDataService.Update(_issueToEdit.Id, _issueToEdit);
        }

        protected override void ResetFields()
        {
            this.Name = "";
            this.Description = "";
            DeadlineDate = null;
            _group = null;
            _issueToEdit = null;
            _isEdit = false;

            base.ResetFields();
        }
    }
}
