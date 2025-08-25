using System.Windows.Input;

namespace PMS.Presentation.Commands
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool> _canExecute;
        private bool _isRunning;

        public AsyncCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute ?? (() => true);
        }

        public bool CanExecute(object parameter)
            => !_isRunning && _canExecute();

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;
            try
            {
                _isRunning = true;
                RaiseCanExecuteChanged();
                await _executeAsync();
            }
            finally
            {
                _isRunning = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
