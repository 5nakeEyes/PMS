using PMS.Domain.Models;
using PMS.Presentation.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PMS.Presentation.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private string _title;
        private DateTime _deadline;
        private TaskState _state;
        private TaskPriority _priority;

        public string Title
        {
            get => _title;
            set
            {
                if (SetProperty(ref _title, value))
                    Model.Title = value;
            }
        }

        // brak właściwości StartDate w TaskModel; korzystamy z Deadline
        public DateTime StartDate => _deadline;

        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                if (SetProperty(ref _deadline, value))
                    Model.Deadline = value;
            }
        }

        public TaskState State
        {
            get => _state;
            set
            {
                if (SetProperty(ref _state, value))
                    Model.State = value;
            }
        }

        public TaskPriority Priority
        {
            get => _priority;
            set
            {
                if (SetProperty(ref _priority, value))
                    Model.Priority = value;
            }
        }

        public ObservableCollection<TaskState> AllStates { get; }
        public ObservableCollection<TaskPriority> AllPriorities { get; }

        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public TaskModel Model { get; }

        public TaskViewModel(
            TaskModel model,
            Action<TaskViewModel> editAction,
            Action<TaskViewModel> deleteAction)
        {
            Model = model;
            _title = model.Title;
            _deadline = model.Deadline;
            _state = model.State;
            _priority = model.Priority;

            AllStates = new ObservableCollection<TaskState>(
                                Enum.GetValues(typeof(TaskState))
                                    .Cast<TaskState>());
            AllPriorities = new ObservableCollection<TaskPriority>(
                                Enum.GetValues(typeof(TaskPriority))
                                    .Cast<TaskPriority>());

            EditCommand = new RelayCommand(_ => editAction(this));
            DeleteCommand = new RelayCommand(_ => deleteAction(this));
        }
    }
}