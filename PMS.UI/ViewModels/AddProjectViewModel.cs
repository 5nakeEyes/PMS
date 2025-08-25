using PMS.Commands;
using System.ComponentModel;
using System.Windows;

namespace PMS.UI.ViewModels
{
    public class AddProjectViewModel : ViewModelBase, IDataErrorInfo
    {
        private string _name;
        private DateTime? _deadline;

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public DateTime? Deadline
        {
            get => _deadline;
            set
            {
                if (_deadline == value) return;
                _deadline = value;
                OnPropertyChanged();
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ConfirmCommand { get; }

        public AddProjectViewModel()
        {
            _deadline = DateTime.Now;
            ConfirmCommand = new RelayCommand(
                execute: o => OnConfirm(o as Window),
                canExecute: _ => CanConfirm
            );
        }

        private bool CanConfirm =>
            !string.IsNullOrWhiteSpace(Name) &&
            Deadline.HasValue;

        private void OnConfirm(Window window)
        {
            if (window == null) return;
            window.DialogResult = true;
            window.Close();
        }

        #region IDataErrorInfo (walidacja)

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Name):
                        return string.IsNullOrWhiteSpace(Name)
                            ? "Nazwa projektu jest wymagana"
                            : null;
                    case nameof(Deadline):
                        return Deadline == null
                            ? "Deadline nie może być pusty"
                            : null;
                    default:
                        return null;
                }
            }
        }

        #endregion
    }
}