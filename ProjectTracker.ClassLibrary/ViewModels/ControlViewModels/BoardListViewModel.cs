using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.ControlViewModels
{
    public class BoardListViewModel : ObservableObject
    {
        #region Fields

        private Project _currentProject;
        private IEnumerable<Board> _boardList;
        private Board _selectedBoard;
        private string _boardSearchText;
        private bool _isEdit;

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
        public string BoardSearchText
        {
            get
            {
                return _boardSearchText;
            }
            set
            {
                _boardSearchText = value;
                RaisePropertyChangedEvent(nameof(BoardSearchText));
                FilterBoardList();
            }
        }

        private IBoardDataService _boardDataService;
        private BoardPopupViewModel _boardPopupViewModel;
        public ICommand CreateBoardCommand { get; private set; }
        public ICommand EditBoardCommand { get; private set; }
        public ICommand DeleteBoardCommand { get; private set; }
        public ICommand OpenBoardCommand { get; private set; }

        public event EventHandler OpenBoardEvent;
        public event EventHandler RefreshBoardEvent;
        
        #endregion 

        public BoardListViewModel(Project currentProject, IBoardDataService boardDataService, BoardPopupViewModel boardPopupViewModel)
        {
            this._currentProject = currentProject;
            this._boardDataService = boardDataService;
            this._boardPopupViewModel = boardPopupViewModel;

            InitialSetup();
        }

        public BoardListViewModel()
        {

        }

        #region Initial Setup
        private void InitialSetup()
        {
            CreateCommands();
            GetBoardList();
        }
        private void CreateCommands()
        {
            OpenBoardCommand = new RelayCommand(OpenBoard);
            CreateBoardCommand = new RelayCommand(ShowCreateBoardPopup);
            EditBoardCommand = new RelayCommand(ShowEditBoardPopup, CanEditOrDeleteBoard);
            DeleteBoardCommand = new RelayCommand(DeleteBoard, CanEditOrDeleteBoard);
        }

        public async void GetBoardList()
        {
            BoardList = await _boardDataService.GetAllInProject(_currentProject.Id);
        }
        #endregion
        private void OpenBoard(object na)
        {
            OpenBoardEvent?.Invoke(this, EventArgs.Empty);
        }
        private void ShowCreateBoardPopup(object na)
        {
            SubscribeToBoardEvents();
            _boardPopupViewModel.ShowCreateBoardPopup(_currentProject);
        }
        public void ShowEditBoardPopup(object na)
        {
            SubscribeToBoardEvents();
            _boardPopupViewModel.ShowEditBoardPopup(SelectedBoard);

            _isEdit = true;
        }
        private async void DeleteBoard(object na)
        {
            await _boardDataService.Delete(SelectedBoard.Id);
            GetBoardList();
            RefreshBoardEvent?.Invoke(this, EventArgs.Empty);
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
                RefreshBoardEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                RefreshBoardEvent?.Invoke(this, EventArgs.Empty);
            }
            GetBoardList();
            UnsubscribeToBoardEvents();

            _isEdit = false;
        }

        private void FilterBoardList()
        {
            BoardList = BoardList.Where(i => i.Name.ToLower().Contains(BoardSearchText.ToLower()) || i.Description.ToLower().Contains(BoardSearchText.ToLower()));
        }
    }
}
