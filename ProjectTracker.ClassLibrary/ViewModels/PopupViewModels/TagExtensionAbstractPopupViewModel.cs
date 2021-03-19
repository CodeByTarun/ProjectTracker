using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public abstract class TagExtensionAbstractPopupViewModel : AbstractPopupViewModel
    {
        private IEnumerable<Tag> _tagList;
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
        public ObservableCollection<Tag> ItemTags { get; set; }
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
                FilterTagList();
            }
        }

        private ITagDataService _tagDataService;

        public ICommand AddItemTagCommand { get; private set; }
        public ICommand RemoveItemTagCommand { get;  private set; }

        public TagExtensionAbstractPopupViewModel(ITagDataService tagDataService) : base()
        {
            _tagDataService = tagDataService;
            ItemTags = new ObservableCollection<Tag>();
            GetTagList();

            AddItemTagCommand = new RelayCommand(AddItemTagToList);
            RemoveItemTagCommand = new RelayCommand(RemoveItemTag);
        }

        private void AddItemTagToList(object selectedTag)
        {
            if (selectedTag != null)
            {
                ItemTags.Add((Tag)selectedTag);
            }
        }

        private void RemoveItemTag(object selectedTag)
        {
            ItemTags.Remove((Tag)selectedTag);
            FilterTagList();
        }

        public async void GetTagList()
        {
            TagList = await _tagDataService.GetAll();
        }

        private void FilterTagList()
        {
            if (TagList != null)
            {
                if (ItemTags.Count() > 0)
                {
                    TagList = TagList.Where(t => t.Name.ToLower().Contains(TagSearchText.ToLower()));
                }
                else
                {
                    TagList = TagList.Where(t => !ItemTags.Any(i => i.Id == t.Id)).Where(t => t.Name.ToLower().Contains(TagSearchText.ToLower()));
                }
            }         
        }

        protected override void ResetFields()
        {
            if (ItemTags != null)
            {
                ItemTags.Clear();
            }

            TagSearchText = "";
        }

    }
}
