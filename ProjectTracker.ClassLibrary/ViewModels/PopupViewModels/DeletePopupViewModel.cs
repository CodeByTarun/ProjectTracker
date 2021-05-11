using ProjectTracker.ClassLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    public class DeletePopupViewModel : ObservableObject
    {
        private string _dialogTitle;
        private bool _isVisible;

        public string DialogTitle
        {
            get
            {
                return _dialogTitle;
            }
            set
            {
                _dialogTitle = value;
                RaisePropertyChangedEvent(nameof(DialogTitle));
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
            }
        }

        public event EventHandler CanceledEvent;
        public event EventHandler DeletedEvent;

        public ICommand CancelCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public DeletePopupViewModel()
        {
            CreateCommands();
            IsVisible = false;
        }

        private void CreateCommands()
        {
            CancelCommand = new RelayCommand(Cancel);
            DeleteCommand = new RelayCommand(Delete);
        }

        private void Delete(object obj)
        {
            IsVisible = false;
            DeletedEvent?.Invoke(this, EventArgs.Empty);
        }

        private void Cancel(object obj)
        {
            IsVisible = false;
            CanceledEvent?.Invoke(this, EventArgs.Empty);
        }

        public void ShowDeleteDialog(string name)
        {
            IsVisible = true;
            DialogTitle = "Delete " + name + "?";
        }
    }
}
