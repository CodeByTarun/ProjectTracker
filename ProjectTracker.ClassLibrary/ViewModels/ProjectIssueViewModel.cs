using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels
{
    public class ProjectIssueViewModel : ObservableObject
    {
        #region Fields

        private Project _currentProject;
        private Board _selectedBoard;
        private IEnumerable<Board> _boardList;
        private bool _isEdit;

        private string _boardName;
        private string _boardDescription;
        private string _boardStatus;

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
                SetBoardFields();
            }
        }
        public IEnumerable<Board> BoardList
        {
            get
            {
                return _boardList;
            }
            set
            {
                _boardList = value;
                RaisePropertyChangedEvent(nameof(BoardList));
            }
        }
        public string BoardName
        {
            get
            {
                return _boardName;
            }
            set
            {
                _boardName = value;
                RaisePropertyChangedEvent(nameof(BoardName));
            }
        }
        public string BoardDescription
        {
            get
            {
                return _boardDescription;
            }
            set
            {
                _boardDescription = value;
                RaisePropertyChangedEvent(nameof(BoardDescription));
            }
        }
        public string BoardStatus
        {
            get
            {
                return _boardStatus;
            }
            set
            {
                _boardStatus = value;
                RaisePropertyChangedEvent(nameof(BoardStatus));
            }
        }

        private IBoardDataService _boardDataService;

        private BoardPopupViewModel _boardPopupViewModel;
        public KanbanControlViewModel KanbanControlViewModel;
        private DeletePopupViewModel _deletePopupViewModel;

        public ICommand CreateBoardCommand { get; private set; }
        public ICommand EditBoardCommand { get; private set; }
        public ICommand DeleteBoardCommand { get; private set; }
        public ICommand OpenBoardCommand { get; private set; }

        public event EventHandler RefreshBoardEvent;
        #endregion

        #region Constructors
        public ProjectIssueViewModel()
        {
            SelectedBoard = new Board()
            {
                Name = "First Board",
                Description = "This is the first of many"
            };
        }
        public ProjectIssueViewModel(Project currentProject, IBoardDataService boardDataService, BoardPopupViewModel boardPopupViewModel, KanbanControlViewModel kanbanControlViewModel, DeletePopupViewModel deletePopupViewModel)
        {
            this._currentProject = currentProject;

            this._boardDataService = boardDataService;
            this._boardPopupViewModel = boardPopupViewModel;
            this.KanbanControlViewModel = kanbanControlViewModel;
            this._deletePopupViewModel = deletePopupViewModel;

            InitialSetup();
        }
        #endregion

        #region Initial Setup
        private void InitialSetup()
        {
            CreateCommands();
            GetBoardList();
            SetBoardFields();
        }

        internal void UpdateTags()
        {
            Board currentBoard = SelectedBoard;
            GetBoardList();

            if (currentBoard != null)
            {
                SelectedBoard = BoardList.FirstOrDefault(b => b.Id == currentBoard.Id);
            }
        }

        private void CreateCommands()
        {
            CreateBoardCommand = new RelayCommand(ShowCreateBoardPopup);
            EditBoardCommand = new RelayCommand(ShowEditBoardPopup, CanEditOrDeleteBoard);
            DeleteBoardCommand = new RelayCommand(ShowDeleteBoardPopup, CanEditOrDeleteBoard);
            OpenBoardCommand = new RelayCommand(OpenBoard);
        }

        private void OpenBoard(object obj)
        {
            SetBoardFields();
        }

        public async void GetBoardList()
        {
            BoardList = await _boardDataService.GetAllInProject(_currentProject.Id);
        }

        private async void SetBoardFields()
        {
            if (SelectedBoard != null)
            {
                BoardName = SelectedBoard.Name;
                BoardDescription = SelectedBoard.Description;
                BoardStatus = SelectedBoard.Status;
                KanbanControlViewModel.SelectedBoard = await _boardDataService.GetBoardWithInnerEntities(SelectedBoard.Id);
            } else
            {
                BoardName = "";
                BoardDescription = "";
                BoardStatus = "";
            }
        }
        #endregion

        #region Board Commands
        private void ShowCreateBoardPopup(object na)
        {
            SubscribeToBoardEvents();
            _boardPopupViewModel.ShowCreateBoardPopup(_currentProject);
        } 
        private void ShowEditBoardPopup(object na)
        {
            SubscribeToBoardEvents();
            _boardPopupViewModel.ShowEditBoardPopup(SelectedBoard);

            _isEdit = true;
        }

        private bool CanEditOrDeleteBoard(object na)
        {
            return (SelectedBoard != null);
        }
        private void SubscribeToBoardEvents()
        {
            _boardPopupViewModel.CreateOrEditEvent += _boardPopupViewModel_CreateOrEditEvent;
            _boardPopupViewModel.ClosePopupEvent += _boardPopupViewModel_ClosePopupEvent;
        }
        private void UnsubscribeToBoardEvents()
        {
            _boardPopupViewModel.CreateOrEditEvent -= _boardPopupViewModel_CreateOrEditEvent;
            _boardPopupViewModel.ClosePopupEvent -= _boardPopupViewModel_ClosePopupEvent;
        }
        private void _boardPopupViewModel_ClosePopupEvent(object sender, EventArgs e)
        {
            UnsubscribeToBoardEvents();

            _isEdit = false;
        }
        private void _boardPopupViewModel_CreateOrEditEvent(object sender, EventArgs e)
        {
            if (_isEdit == false)
            {
                GetBoardList();
                SelectedBoard = BoardList.Last();
            } else
            {
                int boardId = SelectedBoard.Id;
                GetBoardList();
                SelectedBoard = BoardList.FirstOrDefault(b => b.Id == boardId);
            }
            UnsubscribeToBoardEvents();

            RefreshBoardEvent?.Invoke(this, EventArgs.Empty);
            _isEdit = false;
        }

        private void ShowDeleteBoardPopup(object na)
        {
            _deletePopupViewModel.ShowDeleteDialog(SelectedBoard.Name);
            SubscribeToDeleteBoardEvents();
        }

        private void SubscribeToDeleteBoardEvents()
        {
            _deletePopupViewModel.DeletedEvent += _deletePopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent += _deletePopupViewModel_CanceledEvent;
        }
        private void UnsubscribeToDeleteBoardEvents()
        {
            _deletePopupViewModel.DeletedEvent -= _deletePopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent -= _deletePopupViewModel_CanceledEvent;
        }

        private async void _deletePopupViewModel_DeletedEvent(object sender, EventArgs e)
        {
            await _boardDataService.Delete(SelectedBoard.Id);
            GetBoardList();
            SelectedBoard = BoardList.FirstOrDefault();

            RefreshBoardEvent?.Invoke(this, EventArgs.Empty);

            UnsubscribeToDeleteBoardEvents();
        }
        private void _deletePopupViewModel_CanceledEvent(object sender, EventArgs e)
        {
            UnsubscribeToDeleteBoardEvents();
        }

        #endregion
    }
}
