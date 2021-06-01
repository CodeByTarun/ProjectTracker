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
        private Tag _selectedTag;
        private IEnumerable<Tag> _tagList;

        private Group _selectedGroup;
        private Issue _selectedIssue;

        private int? _groupDraggingID;
        private int? _issueDraggingID;

        public int? GroupDraggingID
        {
            get
            {
                return  _groupDraggingID;
            }
            set
            {
                _groupDraggingID = value;
                RaisePropertyChangedEvent(nameof(GroupDraggingID));
            }
        }
        public int? IssueDraggingID
        {
            get
            {
                return _issueDraggingID;
            }
            set
            {
                _issueDraggingID = value;
                RaisePropertyChangedEvent(nameof(IssueDraggingID));
            }
        }

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
                GetTagList();
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

        private IBoardDataService _boardDataService;
        private IGroupDataService _groupDataService;
        private IIssueDataService _issueDataService;

        private GroupPopupViewModel _groupPopupViewModel;
        private IssuePopupViewModel _issuePopupViewModel;
        private DeletePopupViewModel _deletePopupViewModel;

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
        public KanbanControlViewModel(IBoardDataService boardDataService, IGroupDataService groupDataService, IIssueDataService issueDataService, 
            GroupPopupViewModel groupPopupViewModel, IssuePopupViewModel issuePopupViewModel, DeletePopupViewModel deletePopupViewModel)
        {
            _boardDataService = boardDataService;
            _groupDataService = groupDataService;
            _issueDataService = issueDataService;
            _groupPopupViewModel = groupPopupViewModel;
            _issuePopupViewModel = issuePopupViewModel;
            _deletePopupViewModel = deletePopupViewModel;

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
            DeleteGroupCommand = new RelayCommand(ShowDeleteGroupPopup);

            CreateIssueCommand = new RelayCommand(ShowCreateIssuePopup);
            EditIssueCommand = new RelayCommand(ShowEditIssuePopup);
            DeleteIssueCommand = new RelayCommand(ShowDeleteIssuePopup);
        }

        private async void GetBoard()
        {
            SelectedBoard = await _boardDataService.GetBoardWithInnerEntities(SelectedBoard.Id);
        }

        private void GetTagList()
        {
            if (SelectedBoard != null)
            {
                Tag selectedTag = SelectedTag;

                TagList = SelectedBoard.Groups.SelectMany(b => b.Issues).SelectMany(i => i.Tags).GroupBy(t => t.Name).Select(t => t.First()).OrderBy(t => t.Name).ToList();

                if (selectedTag != null)
                {
                    SelectedTag = TagList.FirstOrDefault(t => t.Id == selectedTag.Id);
                }

                if (SelectedTag != null)
                {
                    if (!TagList.Any(t => t.Id == SelectedTag.Id))
                    {
                        SelectedTag = null;
                    }
                }
            }
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

        private void ShowDeleteGroupPopup(object groupToDelete)
        {
            if (groupToDelete != null)
            {
                _selectedGroup = (Group)groupToDelete;
                _deletePopupViewModel.ShowDeleteDialog(_selectedGroup.Name);
                SubscribeToDeleteGroupEvents();
            }
        }

        private void SubscribeToDeleteGroupEvents()
        {
            _deletePopupViewModel.DeletedEvent += _deleteGroupPopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent += _deleteGroupPopupViewModel_CanceledEvent;
        }
        private void UnsubscribeToDeleteGroupEvents()
        {
            _deletePopupViewModel.DeletedEvent -= _deleteGroupPopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent -= _deleteGroupPopupViewModel_CanceledEvent;
        }

        private async void _deleteGroupPopupViewModel_DeletedEvent(object sender, EventArgs e)
        {
            await _groupDataService.Delete((_selectedGroup).Id);
            GetBoard();
            UnsubscribeToDeleteGroupEvents();
            _selectedGroup = null;
        }

        private void _deleteGroupPopupViewModel_CanceledEvent(object sender, EventArgs e)
        {
            UnsubscribeToDeleteGroupEvents();
            _selectedGroup = null;
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

        private void ShowDeleteIssuePopup(object issueToDelete)
        {
            if (issueToDelete != null)
            {
                _selectedIssue = (Issue)issueToDelete;
                SubscribeToDeleteIssueEvents();
                _deletePopupViewModel.ShowDeleteDialog(_selectedIssue.Name);   
            }
        }

        private void SubscribeToDeleteIssueEvents()
        {
            _deletePopupViewModel.DeletedEvent += _deleteIssuePopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent += _deleteIssuePopupViewModel_CanceledEvent;
        }
        private void UnsubscribeToDeleteIssueEvents()
        {
            _deletePopupViewModel.DeletedEvent -= _deleteIssuePopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent -= _deleteIssuePopupViewModel_CanceledEvent;
        }

        private void _deleteIssuePopupViewModel_CanceledEvent(object sender, EventArgs e)
        {
            UnsubscribeToDeleteIssueEvents();
        }
        private async void _deleteIssuePopupViewModel_DeletedEvent(object sender, EventArgs e)
        {
            UnsubscribeToDeleteIssueEvents();
            await _issueDataService.Delete(_selectedIssue.Id);
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

            int indexOfGroup = _selectedBoard.Groups.IndexOf(SelectedBoard.Groups.FirstOrDefault(g => g.Id == issueDragging.GroupID));

            if (issueDragging.GroupID == issueOver.GroupID)
            {
                int indexOfIssueOver = _selectedBoard.Groups[indexOfGroup].Issues.IndexOf(issueOver);
                _selectedBoard.Groups[indexOfGroup].Issues.Remove(issueDragging);
                _selectedBoard.Groups[indexOfGroup].Issues.Insert(indexOfIssueOver, issueDragging);
            }
            else
            {
                int indexOfGroupOver = _selectedBoard.Groups.IndexOf(SelectedBoard.Groups.FirstOrDefault(g => g.Id == issueOver.GroupID));
                int indexOfIssueOver = _selectedBoard.Groups[indexOfGroupOver].Issues.IndexOf(issueOver);

                issueDragging.GroupID = issueOver.GroupID;

                _selectedBoard.Groups[indexOfGroup].Issues.Remove(issueDragging);
                _selectedBoard.Groups[indexOfGroupOver].Issues.Insert(indexOfIssueOver, issueDragging);
            }
            RaisePropertyChangedEvent(nameof(SelectedBoard));
        }
        public void MoveIssueToEnd(Issue issueDragging, Group groupOver)
        {
            int indexOfGroup = _selectedBoard.Groups.IndexOf(_selectedBoard.Groups.FirstOrDefault(g => g.Id == issueDragging.GroupID));
            int indexOfGroupOver = _selectedBoard.Groups.IndexOf(groupOver);

            if (issueDragging.GroupID == groupOver.Id)
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

                issueDragging.GroupID = groupOver.Id;
            }
            RaisePropertyChangedEvent(nameof(SelectedBoard));
        }
        public void MoveIssueInDatabase(Issue issue)
        {
            int indexOfGroupIssueIsMovingTo = SelectedBoard.Groups.IndexOf(SelectedBoard.Groups.FirstOrDefault(g => g.Id == issue.GroupID));

            Issue issueMoving = SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues.FirstOrDefault(i => i.Id == issue.Id);

            int indexOfIssueMoved = SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues.IndexOf(issueMoving);

            Issue issueBefore = null;
            Issue issueAfter = null;

            if (indexOfIssueMoved - 1 >= 0)
            {
                issueAfter = SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues[indexOfIssueMoved - 1];
            }

            if (indexOfIssueMoved + 1 <= SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues.Count() - 1)
            {
                issueBefore = SelectedBoard.Groups[indexOfGroupIssueIsMovingTo].Issues[indexOfIssueMoved + 1];
            }

            _issueDataService.Move(issueMoving, issueBefore, issueAfter, issue.GroupID);
        }
        #endregion

        #region Search Functions
        private void FilterIssueList()
        {
              
        }
        #endregion
    }
}
