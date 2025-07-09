using PMS.Helpers;
using PMS.Core.Models;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace PMS.ViewModels
{
    public class AddTaskViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private DateTime _dueDate = DateTime.Now.AddDays(1);
        private TaskState _state = TaskState.ToDo;
        private TaskPriority _priority = TaskPriority.Medium;

        private readonly Dictionary<string, List<string>> _errors = new();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => _errors.Any();
        public IEnumerable GetErrors(string propertyName)
            => propertyName is not null && _errors.ContainsKey(propertyName)
               ? _errors[propertyName]
               : Enumerable.Empty<string>();

        public string Title
        {
            get => _title;
            set
            {
                if (SetProperty(ref _title, value))
                    ValidateProperty(nameof(Title));
            }

        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (SetProperty(ref _dueDate, value))
                    ValidateProperty(nameof(DueDate));
            }

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
            ConfirmCommand = new RelayCommand(OnConfirm, () => !HasErrors);

            ValidateProperty(nameof(Title));
            ValidateProperty(nameof(DueDate));
        }

        private void ValidateProperty(string propertyName)
        {
            List<string> errors = propertyName switch
            {
                nameof(Title) when string.IsNullOrWhiteSpace(Title) =>
                    new List<string> { "Title is required." },

                nameof(DueDate) when DueDate < DateTime.Now.Date =>
                    new List<string> { "Due date cannot be in the past." },

                _ => new List<string>()
            };

            if (errors.Any())
                _errors[propertyName] = errors;
            else
                _errors.Remove(propertyName);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            ((RelayCommand)ConfirmCommand).RaiseCanExecuteChanged();
        }

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