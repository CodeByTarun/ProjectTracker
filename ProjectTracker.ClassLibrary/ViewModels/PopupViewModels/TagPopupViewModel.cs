using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model.Interfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public class TagPopupViewModel : ObservableObject
    {
        #region Fields
        private IEnumerable<Tag> _tagList;
        private Tag _selectedTag;
        private bool _isVisible;
        private bool _isEdit;

        private string _name;
        private int _color;
        private bool _isFontBlack;
        private string _tagSearchText;

        public IEnumerable<Tag> TagList
        {
            get
            {
                return _tagList;
            }
            set
            {
                _tagList = value;
                RaisePropertyChangedEvent(nameof(TagList));
            }
        }
        public Tag SelectedTag
        {
            get
            {
                return _selectedTag;
            }
            set
            {
                _selectedTag = value;
                RaisePropertyChangedEvent(nameof(SelectedTag));
            }
        }
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                RaisePropertyChangedEvent(nameof(IsVisible));

                if (_isVisible == false)
                {
                    ResetFields();
                }
            }
        }
        public bool IsEdit
        {
            get
            {
                return _isEdit;
            }
            set
            {
                _isEdit = value;
                RaisePropertyChangedEvent(nameof(IsEdit));
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChangedEvent(nameof(Name));
            }
        }
        public int Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                RaisePropertyChangedEvent(nameof(Color));
            }
        }
        public bool IsFontBlack
        {
            get
            {
                return _isFontBlack;
            }
            set
            {
                _isFontBlack = value;
                RaisePropertyChangedEvent(nameof(IsFontBlack));
            }
        }
        public string TagSearchText
        {
            get
            {
                return _tagSearchText;
            }
            set
            {
                _tagSearchText = value;
                RaisePropertyChangedEvent(nameof(TagSearchText));
                FilterTags();
            }
        }

        public ICommand ClosePopupCommand { get; private set; }
        public ICommand CreateTagCommand { get; private set; }
        public ICommand EditTagCommand { get; private set; }
        public ICommand StartEditCommand { get; private set; }
        public ICommand CancelEditCommand { get; private set; }
        public ICommand DeleteTagCommand { get; private set; }

        private ITagDataService _tagDataService;

        public event EventHandler EditOrDeleteTagEvent;
        public event EventHandler AddTagEvent;

        #endregion

        public TagPopupViewModel()
        {
            CreateDummyTagList();
        }

        public TagPopupViewModel(ITagDataService tagDataService)
        {
            this._tagDataService = tagDataService;
            InitialSetup();
        }

        private void CreateDummyTagList()
        {
            List<Tag> Tags = new List<Tag>();

            Tag tag1 = new Tag
            {
                Name = "Gaming",
                Color = 16711935
            };
            Tag tag2 = new Tag
            {
                Name = "Bug",
                Color = 19538596
            };

            Tags.Add(tag1);
            Tags.Add(tag2);

            TagList = Tags;
        }
        private void InitialSetup()
        {
            ResetFields();
            GetTags();
            CreateCommands();
        }
        private async void GetTags()
        {
            TagList = await _tagDataService.GetAll();
        }
        private void CreateCommands()
        {
            ClosePopupCommand = new RelayCommand(ClosePopup);
            CreateTagCommand = new RelayCommand(CreateTag, CanCreateTag);
            EditTagCommand = new RelayCommand(EditTag, CanEditTag);
            StartEditCommand = new RelayCommand(StartEdit);
            CancelEditCommand = new RelayCommand(CancelEdit);
            DeleteTagCommand = new RelayCommand(DeleteTag);
        }
        private void ResetFields()
        {
            Name = "";
            Color = 0;
            IsFontBlack = true;

            TagSearchText = "";

            IsEdit = false;
            SelectedTag = null;
        }

        private async void CreateTag(object obj)
        {
            Tag tag = new Tag
            {
                Name = Name,
                Color = Color,
                IsFontBlack = _isFontBlack
            };

            await _tagDataService.Create(tag);
            GetTags();

            ResetFields();

            AddTagEvent?.Invoke(this, EventArgs.Empty);
        }

        private void StartEdit(object selectedTag)
        {
            IsEdit = true;
            SelectedTag = selectedTag as Tag;

            Name = SelectedTag.Name;
            Color = SelectedTag.Color;
            IsFontBlack = SelectedTag.IsFontBlack;
        }

        private async void EditTag(object obj)
        {
            Tag tag = new Tag
            {
                Name = Name,
                Color = Color,
                IsFontBlack = _isFontBlack
            };

            await _tagDataService.Update(SelectedTag.Id, tag);
            GetTags();

            ResetFields();

            EditOrDeleteTagEvent?.Invoke(this, EventArgs.Empty);

        }

        private void CancelEdit(object obj)
        {
            ResetFields();
        }

        private async void DeleteTag(object SelectedTag)
        {
            await _tagDataService.Delete((SelectedTag as Tag).Id);
            GetTags();

            EditOrDeleteTagEvent?.Invoke(this, EventArgs.Empty);
        }

        private bool CanCreateTag(object obj)
        {
            if (TagList != null)
            {
                return !(TagList.Any(t => t.Name.ToLower() == Name.ToLower()) || Name == "" || Color == 0); 
            } else
            {
                return !(Name == "" || Color == 0);
            }
        }

        private bool CanEditTag(object obj)
        {
            return !(Name == "" || Color == 0);
        }

        private void ClosePopup(object na)
        {
            ResetFields();
            IsVisible = false;
        }
        // TODO
        public void ShowTagPopup()
        {
            IsVisible = true;
        }

        // TODO
        private void FilterTags()
        {
            if (TagList != null)
            {
                TagList = TagList.Where(t => t.Name.ToLower().Contains(TagSearchText.ToLower()));
            }
        }
    }
}