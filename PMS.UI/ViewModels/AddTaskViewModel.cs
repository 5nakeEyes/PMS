using PMS.Commands;
using PMS.Models;
using System.Windows;
using System.Windows.Input;

namespace PMS.UI.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private DateTime? _deadline = DateTime.Today;
        private TaskState _state = TaskState.ToDo;
        private TaskPriority _priority = TaskPriority.Medium;

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
                ((RelayCommand)ConfirmCommand).RaiseCanExecuteChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged();
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
                ((RelayCommand)ConfirmCommand).RaiseCanExecuteChanged();
            }
        }

        public TaskState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                OnPropertyChanged();
            }
        }

        public TaskPriority Priority
        {
            get => _priority;
            set
            {
                if (_priority == value) return;
                _priority = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TaskState> AllStates
        => Enum.GetValues(typeof(TaskState))
               .Cast<TaskState>();

        public IEnumerable<TaskPriority> AllPriorities
            => Enum.GetValues(typeof(TaskPriority))
                   .Cast<TaskPriority>();


        public ICommand ConfirmCommand { get; }

        public AddTaskViewModel()
        {
            ConfirmCommand = new RelayCommand(OnConfirm, _ =>
                !string.IsNullOrWhiteSpace(Title)
                && Deadline.HasValue);
        }

        private void OnConfirm(object parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        public TaskModel ToTaskModel()
        {
            return new TaskModel(
                Title,
                Description,
                Deadline.Value,
                State,
                Priority
            );
        }
    }
}