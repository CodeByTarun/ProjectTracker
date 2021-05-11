using ProjectTracker.ClassLibrary.Helpers;
using ProjectTracker.ClassLibrary.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectTracker.ClassLibrary.ViewModels.PopupViewModels
{
    // To implement:
    // Create constructors, use services required and provide public methods for showing the popup as a create or edit popup

    public abstract class AbstractPopupViewModel : ObservableObject, IPopupViewModel
    {
        protected bool _isEdit;
        private bool _isVisible;
        private string _dialogTitle;
        private string _buttonContent;
        private string _name;

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
        public string ButtonContent
        {
            get
            {
                return _buttonContent;
            }
            set
            {
                _buttonContent = value;
                RaisePropertyChangedEvent(nameof(ButtonContent));
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
        public ICommand ClosePopupCommand { get; private set; }
        public ICommand CreateOrEditCommand { get; private set; }

        public event EventHandler CreateOrEditEvent;
        public event EventHandler ClosePopupEvent;

        protected AbstractPopupViewModel()
        {
            CreateCommands();
            IsVisible = false;
        }

        protected void CreateCommands()
        {
            ClosePopupCommand = new RelayCommand(ClosePopup);
            CreateOrEditCommand = new RelayCommand(CreateOrEditItem, CanCreateorEditItem);
        }
        protected void ClosePopup(object na)
        {
            IsVisible = false;

            ClosePopupEvent?.Invoke(this, EventArgs.Empty);
        }
        protected abstract void ResetFields();

        private async void CreateOrEditItem(object na)
        {
            if (!_isEdit)
            {
                await CreateItem();
            }
            else
            {
                await EditItem();
            }

            CreateOrEditEvent?.Invoke(this, EventArgs.Empty);

            ClosePopup(null);
        }
        private bool CanCreateorEditItem(object na)
        {
            return (Name != "");
        }

        protected abstract Task CreateItem();
        protected abstract Task EditItem();
    }
}
