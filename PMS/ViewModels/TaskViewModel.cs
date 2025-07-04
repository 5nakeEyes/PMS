using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PMS.Models;
using PMS.Helpers;

namespace PMS.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TaskModel> Tasks { get; }
            = new ObservableCollection<TaskModel>();

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get => _selectedTask;
            set
            {
                if (_selectedTask == value) return;
                _selectedTask = value;
                OnPropertyChanged();
            }
        }

        // Pola do tworzenia nowego zadania
        private string _newTitle = string.Empty;
        private string _newDescription = string.Empty;
        private DateTime _newDueDate = DateTime.Now.AddDays(1);
        private TaskState _newState = TaskState.ToDo;
        private TaskPriority _newPriority = TaskPriority.Medium;

        public string NewTitle
        {
            get => _newTitle;
            set { _newTitle = value; OnPropertyChanged(); }
        }

        public string NewDescription
        {
            get => _newDescription;
            set { _newDescription = value; OnPropertyChanged(); }
        }

        public DateTime NewDueDate
        {
            get => _newDueDate;
            set { _newDueDate = value; OnPropertyChanged(); }
        }

        public TaskState NewState
        {
            get => _newState;
            set { _newState = value; OnPropertyChanged(); }
        }

        public TaskPriority NewPriority
        {
            get => _newPriority;
            set { _newPriority = value; OnPropertyChanged(); }
        }

        // Listy enumów do powiązania w ComboBox
        public TaskState[] AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>().ToArray();

        public TaskPriority[] AllPriorities =>
            Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>().ToArray();

        // Komendy
        public ICommand AddTaskCommand { get; }
        public ICommand RemoveTaskCommand { get; }

        public TaskViewModel()
        {
            AddTaskCommand = new RelayCommand(AddTask, CanAddTask);
            RemoveTaskCommand = new RelayCommand(RemoveTask, () => SelectedTask != null);
        }

        private bool CanAddTask()
        {
            // prosta walidacja: tytuł nie może być pusty i termin późniejszy od teraz
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

            // reset pól formularza
            NewTitle = string.Empty;
            NewDescription = string.Empty;
            NewDueDate = DateTime.Now.AddDays(1);
            NewState = TaskState.ToDo;
            NewPriority = TaskPriority.Medium;

            // odśwież CanExecute
            ((RelayCommand)AddTaskCommand).RaiseCanExecuteChanged();
        }

        private void RemoveTask()
        {
            if (SelectedTask == null) return;
            Tasks.Remove(SelectedTask);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
