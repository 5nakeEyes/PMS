using PMS.Application.Repositories;
using PMS.Domain.Models;
using PMS.Presentation.Common;
using PMS.Presentation.Interfaces;

namespace PMS.Presentation.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        private readonly ITaskRepository _repo;
        private readonly IDialogService _dialogs;
        private readonly TaskModel? _editingModel;

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Description { get => _desc; set => SetProperty(ref _desc, value); }

        public DateTime StartDate { get => _startDate; set => SetProperty(ref _startDate, value); }
        public DateTime Deadline { get => _deadline; set => SetProperty(ref _deadline, value); }

        public TaskState State { get => _state; set => SetProperty(ref _state, value); }
        public TaskPriority Priority { get => _priority; set => SetProperty(ref _priority, value); }

        public TaskModel? CreatedTask { get; private set; }

        public IEnumerable<TaskState> AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>();
        public IEnumerable<TaskPriority> AllPriorities =>
            Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>();

        public IAsyncCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }

        public event Action<bool>? RequestClose;

        public AddTaskViewModel(
            ITaskRepository repo,
            IDialogService dialogs)
        {
            _repo = repo;
            _dialogs = dialogs;

            StartDate = DateTime.Today;
            Deadline = DateTime.Today;
            State = TaskState.ToDo;
            Priority = TaskPriority.Medium;

            ConfirmCommand = new AsyncCommand(_ => OnConfirmAsync());
            CancelCommand = new RelayCommand(_ => OnCancel());
        }

        public AddTaskViewModel(
            TaskModel existing,
            ITaskRepository repo,
            IDialogService dialogs)
            : this(repo, dialogs)
        {
            _editingModel = existing;

            _title = existing.Title;
            _desc = existing.Description;
            _startDate = existing.StartDate;
            _deadline = existing.Deadline;
            _state = existing.State;
            _priority = existing.Priority;
        }

        private async Task OnConfirmAsync()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                await _dialogs.ShowConfirmAsync("Validation", "Title is required.");
                return;
            }

            if (_editingModel != null)
            {
                _editingModel.Title = Title;
                _editingModel.Description = Description;
                _editingModel.StartDate = StartDate;
                _editingModel.Deadline = Deadline;
                _editingModel.State = State;
                _editingModel.Priority = Priority;

                await _repo.UpdateAsync(_editingModel);
                CreatedTask = _editingModel;
            }
            else
            {
                var model = new TaskModel(
                    Title, Description,
                    StartDate, Deadline,
                    State, Priority);

                await _repo.AddAsync(model);
                CreatedTask = model;
            }

            RequestClose?.Invoke(true);
        }

        private void OnCancel() => RequestClose?.Invoke(false);

        private string _title = "";
        private string _desc = "";
        private DateTime _startDate;
        private DateTime _deadline;
        private TaskState _state;
        private TaskPriority _priority;
    }
}