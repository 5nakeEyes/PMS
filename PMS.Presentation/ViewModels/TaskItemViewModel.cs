using PMS.Domain.Models;
using PMS.Presentation.Common;

namespace PMS.Presentation.ViewModels
{
    public class TaskItemViewModel : ViewModelBase
    {
        private readonly Action<TaskItemViewModel> _onEdit;
        private readonly Action<TaskItemViewModel> _onDelete;

        public TaskItemViewModel(
            TaskModel model,
            Action<TaskItemViewModel> onEdit,
            Action<TaskItemViewModel> onDelete)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            _onEdit = onEdit ?? throw new ArgumentNullException(nameof(onEdit));
            _onDelete = onDelete ?? throw new ArgumentNullException(nameof(onDelete));

            // Inicjalizacja komend
            EditCommand = new AsyncRelayCommand(ExecuteEditAsync);
            DeleteCommand = new AsyncRelayCommand(ExecuteDeleteAsync);
        }

        public TaskModel Model { get; }

        // Typy komend używamy IAsyncRelayCommand, nie AsyncCommand
        public IAsyncRelayCommand EditCommand { get; }
        public IAsyncRelayCommand DeleteCommand { get; }

        // Metoda wywoływana przez AsyncRelayCommand
        private async Task ExecuteEditAsync()
        {
            // Jeżeli masz logikę w OnEdit, wywołaj callback
            _onEdit?.Invoke(this);
            await Task.CompletedTask;
        }

        private async Task ExecuteDeleteAsync()
        {
            _onDelete?.Invoke(this);
            await Task.CompletedTask;
        }
    }
}