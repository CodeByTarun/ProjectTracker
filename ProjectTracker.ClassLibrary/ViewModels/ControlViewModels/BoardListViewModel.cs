using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using ProjectTracker.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.ControlViewModels
{
    public class BoardListViewModel : ObservableObject
    {
        #region Fields

        private Project _currentProject;
        private string _projectName;
        private string _projectDescription;
        private IEnumerable<Tag> _projectTags;
        private string _projectStatus;

        private IEnumerable<Board> _boardList;
        private Board _selectedBoard;
        private string _boardSearchText;

        private string _selectedStatus;
        public string SelectedStatus
        {
            get
            {
                return _selectedStatus;
            }
            set
            {
                _selectedStatus = value;
                RaisePropertyChangedEvent(nameof(SelectedStatus));
            }
        }

        private IEnumerable<Tag> _tagList;
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

        private Tag _selectedTag;
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

        public Project CurrentProject
        {
            get
            {
                return _currentProject;
            }
            set
            {
                _currentProject = value;
                RaisePropertyChangedEvent(nameof(CurrentProject));
            }
        }
        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                _projectName = value;
                RaisePropertyChangedEvent(nameof(ProjectName));
            }
        }
        public string ProjectDescription
        {
            get
            {
                return _projectDescription;
            }
            set
            {
                _projectDescription = value;
                RaisePropertyChangedEvent(nameof(ProjectDescription));
            }
        }
        public IEnumerable<Tag> ProjectTags
        {
            get
            {
                return _projectTags;
            }
            set
            {
                _projectTags = value;
                RaisePropertyChangedEvent(nameof(ProjectTags));
            }
        }
        public string ProjectStatus
        {
            get
            {
                return _projectStatus;
            }
            set
            {
                _projectStatus = value;
                RaisePropertyChangedEvent(nameof(ProjectStatus));
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

        private IProjectDataService _projectDataService;
        private IBoardDataService _boardDataService;
        private BoardPopupViewModel _boardPopupViewModel;
        private ProjectPopupViewModel _projectPopupViewModel;
        private DeletePopupViewModel _deletePopupViewModel;

        public ICommand CreateBoardCommand { get; private set; }
        public ICommand EditBoardCommand { get; private set; }
        public ICommand DeleteBoardCommand { get; private set; }
        public ICommand OpenBoardCommand { get; private set; }
        public ICommand StatusFilterCommand { get; private set; }
        public ICommand TagFilterCommand { get; private set; }
        public ICommand EditProjectCommnad { get; private set; }
        public ICommand DeleteProjectCommand { get; private set; }

        public event EventHandler OpenBoardEvent;
        public event EventHandler RefreshBoardEvent;
        public event EventHandler ProjectUpdatedEvent;
        public event EventHandler ProjectDeletedEvent;

        #endregion

        #region Constructors
        public BoardListViewModel(Project currentProject, IProjectDataService projectDataService,IBoardDataService boardDataService, 
            BoardPopupViewModel boardPopupViewModel, ProjectPopupViewModel projectPopupViewModel, DeletePopupViewModel deletePopupViewModel)
        {
            this._currentProject = currentProject;
            this._projectDataService = projectDataService;
            this._boardDataService = boardDataService;
            this._boardPopupViewModel = boardPopupViewModel;
            this._projectPopupViewModel = projectPopupViewModel;
            this._deletePopupViewModel = deletePopupViewModel;

            InitialSetup();
        }

        public BoardListViewModel()
        {

        }
        #endregion

        #region Initial Setup
        private void InitialSetup()
        {
            CreateCommands();
            RefreshView();
        }
        private void CreateCommands()
        {
            OpenBoardCommand = new RelayCommand(OpenBoard);
            CreateBoardCommand = new RelayCommand(ShowCreateBoardPopup);
            EditBoardCommand = new RelayCommand(ShowEditBoardPopup, CanEditOrDeleteBoard);
            DeleteBoardCommand = new RelayCommand(ShowDeleteBoardPopup, CanEditOrDeleteBoard);
            StatusFilterCommand = new RelayCommand(StatusFilter);
            TagFilterCommand = new RelayCommand(TagFilter);
            EditProjectCommnad = new RelayCommand(ShowEditProjectPopup);
            DeleteProjectCommand = new RelayCommand(ShowDeleteProjectPopup);
        }

        public async void RefreshView()
        {
            await UpdateProject();
            await GetBoardList();
            GetTagList();
            FilterBoardList();
        }

        public async Task UpdateProject()
        {
            CurrentProject = await _projectDataService.Get(_currentProject.Id);

            ProjectName = CurrentProject.Name;
            ProjectDescription = CurrentProject.Description;
            ProjectTags = CurrentProject.Tags;
            ProjectStatus = CurrentProject.Status;
        }

        public async Task GetBoardList()
        {
            BoardList = await _boardDataService.GetAllInProject(_currentProject.Id);
        }

        private void GetTagList()
        {
            if (BoardList != null)
            {
                Tag selectedTag = SelectedTag;

                TagList = BoardList.SelectMany(b => b.Tags).Distinct().OrderBy(t => t.Name).ToList();

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

        #region Board Commands
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
        }
        private void _boardPopupViewModel_CreateOrEditEvent(object sender, EventArgs e)
        {
            RefreshBoardEvent?.Invoke(this, EventArgs.Empty);
            RefreshView();
            UnsubscribeToBoardEvents();
        }

        private void ShowDeleteBoardPopup(object na)
        {
            _deletePopupViewModel.ShowDeleteDialog(SelectedBoard.Name);
            SubscribeToDeleteBoardEvents();
        }

        private void SubscribeToDeleteBoardEvents()
        {
            _deletePopupViewModel.DeletedEvent += _deleteBoardPopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent += _deleteBoardPopupViewModel_CanceledEvent;
        }
        private void UnsubscribeToDeleteBoardEvents()
        {
            _deletePopupViewModel.DeletedEvent -= _deleteBoardPopupViewModel_DeletedEvent;
            _deletePopupViewModel.CanceledEvent -= _deleteBoardPopupViewModel_CanceledEvent;
        }
        private async void _deleteBoardPopupViewModel_DeletedEvent(object sender, EventArgs e)
        {
            await _boardDataService.Delete(SelectedBoard.Id);
            RefreshView();
            RefreshBoardEvent?.Invoke(this, EventArgs.Empty);
            UnsubscribeToDeleteBoardEvents();
        }
        private void _deleteBoardPopupViewModel_CanceledEvent(object sender, EventArgs e)
        {
            UnsubscribeToDeleteBoardEvents();
        }

        
        #endregion

        #region Project Commands
        private void ShowEditProjectPopup(object obj)
        {
            SubscribeToProjectEvents();
            _projectPopupViewModel.ShowEditProjectPopup(CurrentProject);
        }

        private void SubscribeToProjectEvents()
        {
            _projectPopupViewModel.CreateOrEditEvent += _projectPopupViewModel_CreateOrEditEvent;
            _projectPopupViewModel.ClosePopupEvent += _projectPopupViewModel_ClosePopupEvent;
        }
        private void UnsubscribeToProjectEvents()
        {
            _projectPopupViewModel.CreateOrEditEvent -= _projectPopupViewModel_CreateOrEditEvent;
            _projectPopupViewModel.ClosePopupEvent -= _projectPopupViewModel_ClosePopupEvent;
        }

        private void _projectPopupViewModel_CreateOrEditEvent(object sender, EventArgs e)
        {
            RefreshView();
            ProjectUpdatedEvent?.Invoke(this, EventArgs.Empty);
            UnsubscribeToProjectEvents();
        }

        private void _projectPopupViewModel_ClosePopupEvent(object sender, EventArgs e)
        {
            UnsubscribeToProjectEvents();
        }

        private void ShowDeleteProjectPopup(object obj)
        {
            SubscribeToDeleteProjectEvents();
            _deletePopupViewModel.ShowDeleteDialog(ProjectName);
        }

        private void SubscribeToDeleteProjectEvents()
        {
            _deletePopupViewModel.DeletedEvent += _deletePopupViewModel_ProjectDeletedEvent;
            _deletePopupViewModel.CanceledEvent += _deletePopupViewModel_CanceledEvent;
        }

        private void UnsubscribeToDeleteProjectEvents()
        {
            _deletePopupViewModel.DeletedEvent -= _deletePopupViewModel_ProjectDeletedEvent;
            _deletePopupViewModel.CanceledEvent -= _deletePopupViewModel_CanceledEvent;
        }

        private async void _deletePopupViewModel_ProjectDeletedEvent(object sender, EventArgs e)
        {
            await _projectDataService.Delete(CurrentProject.Id);
            ProjectDeletedEvent?.Invoke(this, EventArgs.Empty);
            UnsubscribeToDeleteProjectEvents();
        }

        private void _deletePopupViewModel_CanceledEvent(object sender, EventArgs e)
        {
            UnsubscribeToDeleteProjectEvents();
        }

        #endregion

        private void FilterBoardList()
        {
            if (BoardList != null)
            {
                BoardList = BoardList
                            .Where(i => SelectedStatus == null || i.Status.Equals(SelectedStatus))
                            .Where(i => SelectedTag == null || i.Tags.Any(i => i.Id == SelectedTag.Id))
                            .Where(i => BoardSearchText == null || (i.Name.ToLower().Contains(BoardSearchText.ToLower()) || i.Description.ToLower().Contains(BoardSearchText.ToLower())));
            }
        }
        private void TagFilter(object obj)
        {
            FilterBoardList();
        }
        private void StatusFilter(object obj)
        {
            FilterBoardList();
        }
    }
}
