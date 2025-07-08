using PMS.Helpers;
using PMS.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PMS.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        public ObservableCollection<TaskModel> Tasks { get; }
            = new ObservableCollection<TaskModel>();

        private TaskModel? _selectedTask;
        public TaskModel? SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (SetProperty(ref _selectedTask, value))
                    ((RelayCommand)RemoveTaskCommand).RaiseCanExecuteChanged();
            }
        }

        // form fields to create a new task
        private string _newTitle = string.Empty;
        private string _newDescription = string.Empty;
        private DateTime _newDueDate = DateTime.Now.AddDays(1);
        private TaskState _newState = TaskState.ToDo;
        private TaskPriority _newPriority = TaskPriority.Medium;

        public string NewTitle
        {
            get => _newTitle;
            set => SetProperty(ref _newTitle, value);
        }

        public string NewDescription
        {
            get => _newDescription;
            set => SetProperty(ref _newDescription, value);
        }

        public DateTime NewDueDate
        {
            get => _newDueDate;
            set => SetProperty(ref _newDueDate, value);
        }

        public TaskState NewState
        {
            get => _newState;
            set => SetProperty(ref _newState, value);
        }

        public TaskPriority NewPriority
        {
            get => _newPriority;
            set => SetProperty(ref _newPriority, value);
        }

        public TaskState[] AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>().ToArray();

        public TaskPriority[] AllPriorities =>
            Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>().ToArray();

        public ICommand AddTaskCommand { get; }
        public ICommand RemoveTaskCommand { get; }

        public TaskViewModel()
        {
            AddTaskCommand = new RelayCommand(AddTask, CanAddTask);
            RemoveTaskCommand = new RelayCommand(RemoveTask, () => SelectedTask != null);
        }

        private bool CanAddTask()
        {
            return !string.IsNullOrWhiteSpace(NewTitle)
                   && NewDueDate >= DateTime.Now.Date;
        }

        private void AddTask()
        {
            var task = new TaskModel(
                title: NewTitle,
                description: NewDescription,
                dueDate: NewDueDate,
                state: NewState,
                priority: NewPriority);

            Tasks.Add(task);
            SelectedTask = task;

            // reset form fields
            NewTitle = string.Empty;
            NewDescription = string.Empty;
            NewDueDate = DateTime.Now.AddDays(1);
            NewState = TaskState.ToDo;
            NewPriority = TaskPriority.Medium;

            ((RelayCommand)AddTaskCommand).RaiseCanExecuteChanged();
        }

        private void RemoveTask()
        {
            if (SelectedTask == null) return;
            Tasks.Remove(SelectedTask);
        }
    }
}