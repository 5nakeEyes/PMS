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

        // Gdy nie-null => edycja, gdy null => dodawanie
        private readonly TaskModel? _editingModel;

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Description { get => _desc; set => SetProperty(ref _desc, value); }
        public DateTime DueDate { get => _dueDate; set => SetProperty(ref _dueDate, value); }
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

        // Konstruktor do nowego zadania
        public AddTaskViewModel(
            ITaskRepository repo,
            IDialogService dialogs)
        {
            _repo = repo;
            _dialogs = dialogs;

            DueDate = DateTime.Today;
            State = TaskState.ToDo;
            Priority = TaskPriority.Medium;

            ConfirmCommand = new AsyncCommand(_ => OnConfirmAsync());
            CancelCommand = new RelayCommand(_ => OnCancel());
        }

        // Konstruktor do edycji – przekazuję istniejący model
        public AddTaskViewModel(
            TaskModel existing,
            ITaskRepository repo,
            IDialogService dialogs)
            : this(repo, dialogs)
        {
            _editingModel = existing;

            // Wczytujemy wartości z modelu
            _title = existing.Title;
            _desc = existing.Description;
            _dueDate = existing.DueDate;
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
                // Edycja: nadpisujemy dane w istniejącym modelu
                _editingModel.Title = Title;
                _editingModel.Description = Description;
                _editingModel.DueDate = DueDate;
                _editingModel.State = State;
                _editingModel.Priority = Priority;

                await _repo.UpdateAsync(_editingModel);
                CreatedTask = _editingModel;
            }
            else
            {
                // Dodawanie nowego wpisu
                var model = new TaskModel(Title, Description, DueDate, State, Priority);
                await _repo.AddAsync(model);
                CreatedTask = model;
            }

            RequestClose?.Invoke(true);
        }

        private void OnCancel()
        {
            RequestClose?.Invoke(false);
        }

        private string _title = "";
        private string _desc = "";
        private DateTime _dueDate;
        private TaskState _state;
        private TaskPriority _priority;
    }
}