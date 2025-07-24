using PMS.Application.Repositories;
using PMS.Domain.Models;
using PMS.Presentation.Common;
using PMS.Presentation.Interfaces;

namespace PMS.Presentation.ViewModels
{
    public class TaskItemViewModel : ViewModelBase
    {
        private readonly ITaskRepository _repo;
        private readonly IDialogService _dialogs;

        public TaskModel Model { get; }

        public int Id => Model.Id;

        public IEnumerable<TaskState> AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>();

        public IEnumerable<TaskPriority> AllPriorities =>
            Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>();

        public string Title
        {
            get => Model.Title;
            set
            {
                if (Model.Title != value)
                {
                    Model.Title = value;
                    OnPropertyChanged();
                    _ = UpdateAsync();
                }
            }
        }

        public DateTime DueDate
        {
            get => Model.DueDate;
            set
            {
                if (Model.DueDate != value)
                {
                    Model.DueDate = value;
                    OnPropertyChanged();
                    _ = UpdateAsync();
                }
            }
        }

        public TaskState State
        {
            get => Model.State;
            set
            {
                if (Model.State != value)
                {
                    Model.State = value;
                    OnPropertyChanged();
                    _ = UpdateAsync();
                }
            }
        }

        public TaskPriority Priority
        {
            get => Model.Priority;
            set
            {
                if (Model.Priority != value)
                {
                    Model.Priority = value;
                    OnPropertyChanged();
                    _ = UpdateAsync();
                }
            }
        }

        public IAsyncCommand EditCommand { get; }
        public IAsyncCommand DeleteCommand { get; }

        public event Action<TaskItemViewModel>? Deleted;

        public TaskItemViewModel(
            TaskModel model,
            ITaskRepository repo,
            IDialogService dialogs)
        {
            Model = model;
            _repo = repo;
            _dialogs = dialogs;

            EditCommand = new AsyncCommand(_ => OnEditAsync());
            DeleteCommand = new AsyncCommand(_ => OnDeleteAsync());
        }

        private async Task UpdateAsync()
        {
            await _repo.UpdateAsync(Model);
        }

        private async Task OnEditAsync()
        {
            var vm = new AddTaskViewModel(Model, _repo, _dialogs);

            var result = await _dialogs.ShowDialogAsync(vm);
            if (result && vm.CreatedTask is TaskModel updated)
            {
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(DueDate));
                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(Priority));
            }
        }

        private async Task OnDeleteAsync()
        {
            var confirm = await _dialogs.ShowConfirmAsync(
                "Usuń zadanie",
                $"Czy na pewno chcesz usunąć zadanie '{Model.Title}'?");
            if (!confirm) return;

            await _repo.DeleteAsync(Model.Id);
            Deleted?.Invoke(this);
        }
    }
}