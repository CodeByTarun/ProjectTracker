using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.ControlViewModels
{
    public class KanbanControlViewModel : ObservableObject
    {
        #region Fields
        private Board _selectedBoard;
        private ObservableCollection<Group> _groupList;

        public Board SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                RaisePropertyChangedEvent(nameof(SelectedBoard));
            }
        }
        public ObservableCollection<Group> GroupList
        {
            get
            {
                return _groupList;
            }
            set
            {
                _groupList = value;
                RaisePropertyChangedEvent(nameof(GroupList));
            }
        }

        private IGroupDataService _groupDataService;
        private IIssueDataService _issueDataService;

        private GroupPopupViewModel _groupPopupViewModel;
        private IssuePopupViewModel _issuePopupViewModel;

        public ICommand CreateGroupCommand { get; private set; }
        public ICommand EditGroupCommand { get; private set; }
        public ICommand DeleteGroupCommand { get; private set; }

        public ICommand CreateIssueCommand { get; private set; }
        public ICommand EditIssueCommand { get; private set; }
        public ICommand DeleteIssueCommand { get; private set; }
        #endregion

        #region Constructors
        public KanbanControlViewModel()
        {
            GroupList = CreateGroupList();
        }
        public KanbanControlViewModel(IGroupDataService groupDataService, IIssueDataService issueDataService, GroupPopupViewModel groupPopupViewModel, IssuePopupViewModel issuePopupViewModel)
        {
            _groupDataService = groupDataService;
            _issueDataService = issueDataService;
            _groupPopupViewModel = groupPopupViewModel;
            _issuePopupViewModel = issuePopupViewModel;

            InitialSetup();
            GroupList = CreateGroupList();
        }
        #endregion

        #region Design Dummy Data
        private ObservableCollection<Group> CreateGroupList()
        {
            Group group1 = new Group()
            {
                BoardID = 1,
                Name = "To Do",
                Id = 1
            };
            Group group2 = new Group()
            {
                BoardID = 1,
                Name = "In Progress",
                Id = 2
            };
            Group group3 = new Group()
            {
                BoardID = 1,
                Name = "Completed",
                Id = 3
            };

            Issue issue = new Issue
            {
                Name = "First Issue",
                Description = "Testing",
                Tag = "Bug",
                DateCreated = DateTime.Now,
                GroupID = 1,
                Group = group1,
                Id = 1
            };
            Issue issue2 = new Issue
            {
                Name = "Second Issue",
                Description = "Testing",
                Tag = "TODO",
                DateCreated = DateTime.Now,
                GroupID = 1,
                Group = group1,
                Id = 2
            };
            Issue issue3 = new Issue
            {
                Name = "Third Issue",
                Description = "Testing",
                Tag = "Bug",
                DateCreated = DateTime.Now,
                GroupID = 1,
                Group = group1,
                Id = 3
            };

            Issue issue4 = new Issue
            {
                Name = "Fourth Issue",
                Description = "Testing",
                Tag = "Bug",
                DateCreated = DateTime.Now,
                GroupID = 2,
                Group = group2,
                Id = 4
            };
            Issue issue5 = new Issue
            {
                Name = "Fifth Issue",
                Description = "Testing",
                Tag = "TODO",
                DateCreated = DateTime.Now,
                GroupID = 2,
                Group = group2,
                Id = 5
            };
            Issue issue6 = new Issue
            {
                Name = "Sixth Issue",
                Description = "Testing",
                Tag = "Bug",
                DateCreated = DateTime.Now,
                GroupID = 3,
                Group = group3,
                Id = 6
            };

            ObservableCollection<Issue> issuesGroup1 = new ObservableCollection<Issue>()
            {
                issue,
                issue2,
                issue3
            };
            ObservableCollection<Issue> issuesGroup2 = new ObservableCollection<Issue>()
            {
                issue4,
                issue5
            };
            ObservableCollection<Issue> issuesGroup3 = new ObservableCollection<Issue>()
            {
                issue6
            };

            group1.Issues = issuesGroup1;
            group2.Issues = issuesGroup2;
            group3.Issues = issuesGroup3;

            ObservableCollection<Group> groups = new ObservableCollection<Group>()
            {
                group1,
                group2,
                group3
            };

            return groups;
        }
        #endregion

        #region Initial Setup
        private void InitialSetup()
        {
            CreateCommands();
        }
        private void CreateCommands()
        {
            CreateGroupCommand = new RelayCommand(ShowCreateGroupPopup);
            EditGroupCommand = new RelayCommand(ShowEditGroupPopup);
            DeleteGroupCommand = new RelayCommand(DeleteGroup);

            CreateIssueCommand = new RelayCommand(ShowCreateIssuePopup);
            EditIssueCommand = new RelayCommand(ShowEditIssuePopup);
            DeleteIssueCommand = new RelayCommand(DeleteIssue);
        }

        private void GetBoard()
        {

        }
        #endregion

        #region Group Commands
        private void ShowCreateGroupPopup(object na)
        {
            GroupSubscribeToEvents();
            _groupPopupViewModel.ShowCreateGroupPopup(_selectedBoard);
        }
        private void ShowEditGroupPopup(object groupToEdit)
        {
            GroupSubscribeToEvents();
            _groupPopupViewModel.ShowEditGroupPopup((Group)groupToEdit, _selectedBoard);
        }
        /// TODO
        private void DeleteGroup(object na)
        {

        }

        private void GroupSubscribeToEvents()
        {
            _groupPopupViewModel.CreateOrEditEvent += _groupPopupViewModel_CreateOrEditEvent;
            _groupPopupViewModel.ClosePopupEvent += _groupPopupViewModel_ClosePopupEvent;
        }
        private void GroupUnsubscribeToEvents()
        {
            _groupPopupViewModel.CreateOrEditEvent -= _groupPopupViewModel_CreateOrEditEvent;
            _groupPopupViewModel.ClosePopupEvent -= _groupPopupViewModel_ClosePopupEvent;
        }

        private void _groupPopupViewModel_ClosePopupEvent(object sender, EventArgs e)
        {
            GroupUnsubscribeToEvents();

            GetBoard();
        }
        private void _groupPopupViewModel_CreateOrEditEvent(object sender, EventArgs e)
        {
            GroupUnsubscribeToEvents();
        }

        #endregion

        #region Issue Commands
        /// TODO
        private void ShowCreateIssuePopup(object na)
        {

        }
        /// TODO
        private void ShowEditIssuePopup(object na)
        {

        }
        /// TODO
        private void DeleteIssue(object na)
        {

        }
        #endregion

        #region Drag and Drop Functions
        public void MoveGroups(Group groupDragging, Group groupOver)
        {
            int indexDragging = GroupList.IndexOf(groupDragging);
            int indexOver = GroupList.IndexOf(groupOver);

            Group group = _groupList[indexDragging];
            _groupList.RemoveAt(indexDragging);
            _groupList.Insert(indexOver, group);

            RaisePropertyChangedEvent(nameof(GroupList));
        }

        public void MoveIssues(Issue issueDragging, Issue issueOver)
        {
            // Two cases:
            // 1. Both issues are in the same group
            // 2. Issues are in different groups 
            // - remove from existing group and add to group of element dragged over.

            int indexOfGroup = _groupList.IndexOf(issueDragging.Group);

            if (issueDragging.Group == issueOver.Group)
            {
                int indexOfIssueOver = _groupList[indexOfGroup].Issues.IndexOf(issueOver);
                _groupList[indexOfGroup].Issues.Remove(issueDragging);
                _groupList[indexOfGroup].Issues.Insert(indexOfIssueOver, issueDragging);
            }
            else
            {
                int indexOfGroupOver = _groupList.IndexOf(issueOver.Group);
                int indexOfIssueOver = _groupList[indexOfGroupOver].Issues.IndexOf(issueOver);

                issueDragging.Group = issueOver.Group;

                _groupList[indexOfGroup].Issues.Remove(issueDragging);
                _groupList[indexOfGroupOver].Issues.Insert(indexOfIssueOver, issueDragging);
            }
            RaisePropertyChangedEvent(nameof(GroupList));
        }

        public void MoveIssueToEnd(Issue issueDragging, Group groupOver)
        {
            int indexOfGroup = _groupList.IndexOf(issueDragging.Group);
            int indexOfGroupOver = _groupList.IndexOf(groupOver);

            if (issueDragging.Group == groupOver)
            {
                if (_groupList[indexOfGroup].Issues.Last() != issueDragging)
                {
                    _groupList[indexOfGroup].Issues.Remove(issueDragging);
                    _groupList[indexOfGroup].Issues.Add(issueDragging);
                }
                else
                {
                    return;
                }
            }
            else
            {
                _groupList[indexOfGroup].Issues.Remove(issueDragging);
                _groupList[indexOfGroupOver].Issues.Add(issueDragging);

                issueDragging.Group = groupOver;
            }
            RaisePropertyChangedEvent(nameof(GroupList));
        }

        #endregion
    }
}
