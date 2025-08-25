using PMS.Presentation.Common;
using System.Windows.Input;

namespace PMS.Presentation.ViewModels
{
    public class AddProjectViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
                (ConfirmCommand as RelayCommand)?
                    .RaiseCanExecuteChanged();
            }
        }

        public DateTime Deadline { get; set; } = DateTime.Today;

        public event EventHandler<bool>? CloseRequested;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public AddProjectViewModel()
        {
            ConfirmCommand = new RelayCommand(_ => OnConfirm(), _ => CanConfirm());
            CancelCommand = new RelayCommand(_ => OnCancel());
        }

        private bool CanConfirm()
            => !string.IsNullOrWhiteSpace(Name);

        private void OnConfirm()
            => CloseRequested?.Invoke(this, true);

        private void OnCancel()
            => CloseRequested?.Invoke(this, false);
    }
}