using PMS.Helpers;
using PMS.Models;
using PMS.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

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
            set => SetProperty(ref _selectedTask, value);
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

        public ICommand ShowAddDialogCommand { get; }
        public ICommand AddTaskCommand { get; }

        public TaskViewModel()
        {
            ShowAddDialogCommand = new RelayCommand(OpenAddDialog);
            AddTaskCommand = new RelayCommand(AddTask, CanAddTask);
        }
        private void OpenAddDialog()
        {
            var vm = new AddTaskViewModel();
            var win = new AddTaskWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (win.ShowDialog() == true && vm.CreatedTask != null)
            {
                Tasks.Add(vm.CreatedTask);
                SelectedTask = vm.CreatedTask;
            }
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
    }
}