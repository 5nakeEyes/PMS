using PMS.Commands;
using System;
using System.Windows.Input;

namespace PMS.UI.ViewModels
{
    public class ConfirmationDialogViewModel : ViewModelBase
    {
        public string Message { get; }
        public ICommand YesCommand { get; }
        public ICommand NoCommand { get; }

        public event Action<bool> RequestClose;

        public ConfirmationDialogViewModel(string message)
        {
            Message = message;

            YesCommand = new RelayCommand(_ => OnRequestClose(true));
            NoCommand = new RelayCommand(_ => OnRequestClose(false));
        }

        private void OnRequestClose(bool result)
        {
            RequestClose?.Invoke(result);
        }
    }
}