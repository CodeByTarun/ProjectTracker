using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.ControlViewModels
{
    public class KanbanControlViewModel : ObservableObject
    {
        #region Fields
        private Board _selectedBoard;
        private string _issueSearchText;

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
        public string IssueSearchText
        {
            get
            {
                return _issueSearchText;
            }
            set
            {
                _issueSearchText = value;
                RaisePropertyChangedEvent(nameof(IssueSearchText));
            }
        }

        private IBoardDataService _boardDataService;
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
            _selectedBoard.Groups = CreateGroupList();
        }
        public KanbanControlViewModel(IBoardDataService boardDataService, IGroupDataService groupDataService, IIssueDataService issueDataService, GroupPopupViewModel groupPopupViewModel, IssuePopupViewModel issuePopupViewModel)
        {
            _boardDataService = boardDataService;
            _groupDataService = groupDataService;
            _issueDataService = issueDataService;
            _groupPopupViewModel = groupPopupViewModel;
            _issuePopupViewModel = issuePopupViewModel;

            InitialSetup();
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
                DateCreated = DateTime.Now,
                GroupID = 1,
                Group = group1,
                Id = 1
            };
            Issue issue2 = new Issue
            {
                Name = "Second Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 1,
                Group = group1,
                Id = 2
            };
            Issue issue3 = new Issue
            {
                Name = "Third Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 1,
                Group = group1,
                Id = 3
            };

            Issue issue4 = new Issue
            {
                Name = "Fourth Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 2,
                Group = group2,
                Id = 4
            };
            Issue issue5 = new Issue
            {
                Name = "Fifth Issue",
                Description = "Testing",
                DateCreated = DateTime.Now,
                GroupID = 2,
                Group = group2,
                Id = 5
            };
            Issue issue6 = new Issue
            {
                Name = "Sixth Issue",
                Description = "Testing",
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

        private async void GetBoard()
        {
            SelectedBoard = await _boardDataService.GetBoardWithInnerEntities(SelectedBoard.Id);
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
            _groupPopupViewModel.ShowEditGroupPopup((Group)groupToEdit);
        }
        /// TODO
        private async void DeleteGroup(object groupToDelete)
        {
            await _groupDataService.Delete((groupToDelete as Group).Id);
            GetBoard();
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
        }
        private void _groupPopupViewModel_CreateOrEditEvent(object sender, EventArgs e)
        {
            GroupUnsubscribeToEvents();
            GetBoard();
        }

        #endregion

        #region Issue Commands
        private void ShowCreateIssuePopup(object group)
        {
            IssueSubscribeToEvents();
            _issuePopupViewModel.ShowCreateIssuePopup((Group)group);
        }
        private void ShowEditIssuePopup(object issueToEdit)
        {
            IssueSubscribeToEvents();
            _issuePopupViewModel.ShowEditIssuePopup((Issue)issueToEdit);
        }
        /// TODO
        private async void DeleteIssue(object issueToDelete)
        {
            await _issueDataService.Delete((issueToDelete as Issue).Id);
            GetBoard();
        }
        
        private void IssueSubscribeToEvents()
        {
            _issuePopupViewModel.CreateOrEditEvent += _issuePopupViewModel_CreateOrEditEvent;
            _issuePopupViewModel.ClosePopupEvent += _issuePopupViewModel_ClosePopupEvent;
        }
        private void IssueUnsubscribeToEvents()
        {
            _issuePopupViewModel.CreateOrEditEvent -= _issuePopupViewModel_CreateOrEditEvent;
            _issuePopupViewModel.ClosePopupEvent -= _issuePopupViewModel_ClosePopupEvent;
        }

        private void _issuePopupViewModel_ClosePopupEvent(object sender, EventArgs e)
        {
            IssueUnsubscribeToEvents();
        }
        private void _issuePopupViewModel_CreateOrEditEvent(object sender, EventArgs e)
        {
            IssueUnsubscribeToEvents();
            GetBoard();
        }

        
        #endregion

        #region Drag and Drop Functions
        public void MoveGroups(Group groupDragging, Group groupOver)
        {
            int indexDragging = SelectedBoard.Groups.IndexOf(groupDragging);
            int indexOver = SelectedBoard.Groups.IndexOf(groupOver);

            Group group = _selectedBoard.Groups[indexDragging];
            _selectedBoard.Groups.RemoveAt(indexDragging);
            _selectedBoard.Groups.Insert(indexOver, group);

            RaisePropertyChangedEvent(nameof(SelectedBoard));
        }
        public void MoveGroupInDatabase(Group groupMoving)
        {
            int indexOfGroupMoved = SelectedBoard.Groups.IndexOf(groupMoving);

            Group groupBefore = null;
            Group groupAfter = null;

            if (indexOfGroupMoved - 1 >= 0)
            {
                groupBefore = SelectedBoard.Groups[indexOfGroupMoved - 1];
            }

            if (indexOfGroupMoved + 1 <= SelectedBoard.Groups.Count() - 1)
            {
                groupAfter = SelectedBoard.Groups[indexOfGroupMoved + 1];
            }

            _groupDataService.Move(groupMoving, groupBefore, groupAfter);
        }

        public void MoveIssues(Issue issueDragging, Issue issueOver)
        {
            // Two cases:
            // 1. Both issues are in the same group
            // 2. Issues are in different groups 
            // - remove from existing group and add to group of element dragged over.

            int indexOfGroup = _selectedBoard.Groups.IndexOf(issueDragging.Group);

            if (issueDragging.Group == issueOver.Group)
            {
                int indexOfIssueOver = _selectedBoard.Groups[indexOfGroup].Issues.IndexOf(issueOver);
                _selectedBoard.Groups[indexOfGroup].Issues.Remove(issueDragging);
                _selectedBoard.Groups[indexOfGroup].Issues.Insert(indexOfIssueOver, issueDragging);
            }
            else
            {
                int indexOfGroupOver = _selectedBoard.Groups.IndexOf(issueOver.Group);
                int indexOfIssueOver = _selectedBoard.Groups[indexOfGroupOver].Issues.IndexOf(issueOver);

                issueDragging.Group = issueOver.Group;

                _selectedBoard.Groups[indexOfGroup].Issues.Remove(issueDragging);
                _selectedBoard.Groups[indexOfGroupOver].Issues.Insert(indexOfIssueOver, issueDragging);
            }
            RaisePropertyChangedEvent(nameof(SelectedBoard));
        }
        public void MoveIssueToEnd(Issue issueDragging, Group groupOver)
        {
            int indexOfGroup = _selectedBoard.Groups.IndexOf(issueDragging.Group);
            int indexOfGroupOver = _selectedBoard.Groups.IndexOf(groupOver);

            if (issueDragging.Group == groupOver)
            {
                if (_selectedBoard.Groups[indexOfGroup].Issues.Last() != issueDragging)
                {
                    _selectedBoard.Groups[indexOfGroup].Issues.Remove(issueDragging);
                    _selectedBoard.Groups[indexOfGroup].Issues.Add(issueDragging);
                }
                else
                {
                    return;
                }
            }
            else
            {
                _selectedBoard.Groups[indexOfGroup].Issues.Remove(issueDragging);
                _selectedBoard.Groups[indexOfGroupOver].Issues.Add(issueDragging);

                issueDragging.Group = groupOver;
            }
            RaisePropertyChangedEvent(nameof(SelectedBoard));
        }
        public void MoveIssueInDatabase(Issue issue)
        {
            int indexOfGroupIssueIsMovingTo = SelectedBoard.Groups.IndexOf(SelectedBoard.Groups.FirstOrDefault(g => g.Id == issue.Group.Id));

            Issue issueMoving = SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues.FirstOrDefault(i => i.Id == issue.Id);

            int indexOfIssueMoved = SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues.IndexOf(issueMoving);

            Issue issueBefore = null;
            Issue issueAfter = null;

            if (indexOfIssueMoved - 1 >= 0)
            {
                issueBefore = SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues[indexOfIssueMoved - 1];
            }

            if (indexOfIssueMoved + 1 <= SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues.Count() - 1)
            {
                issueAfter = SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues[indexOfIssueMoved + 1];
            }

            _issueDataService.Move(issueMoving, issueBefore, issueAfter, issue.Group.Id);
        }
        #endregion

        #region Search Functions
        private void FilterIssueList()
        {
              
        }
        #endregion
    }
}
