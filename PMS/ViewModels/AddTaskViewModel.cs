using PMS.Helpers;
using PMS.Models;
using System.Windows.Input;

namespace PMS.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private DateTime _dueDate = DateTime.Now.AddDays(1);
        private TaskState _state = TaskState.ToDo;
        private TaskPriority _priority = TaskPriority.Medium;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set => SetProperty(ref _dueDate, value);
        }

        public TaskState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public TaskPriority Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }

        public TaskState[] AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>().ToArray();

        public TaskPriority[] AllPriorities =>
            Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>().ToArray();

        public TaskModel? CreatedTask { get; private set; }

        public ICommand ConfirmCommand { get; }

        public event Action<bool?>? RequestClose;

        public AddTaskViewModel()
        {
            ConfirmCommand = new RelayCommand(OnConfirm, CanConfirm);
        }

        private bool CanConfirm() => !string.IsNullOrWhiteSpace(Title)
                                     && DueDate >= DateTime.Now.Date;

        private void OnConfirm()
        {
            CreatedTask = new TaskModel(
                title: Title,
                description: Description,
                dueDate: DueDate,
                state: State,
                priority: Priority);

            RequestClose?.Invoke(true);
        }
    }
}