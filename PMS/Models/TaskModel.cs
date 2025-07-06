using System;
using System.ComponentModel;


namespace PMS.Models
{
    public enum TaskState
    {
        ToDo,
        InProgress,
        Done
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public class TaskModel : INotifyPropertyChanged
    {
        private string _title;
        private string _description;
        private DateTime _dueDate;
        private TaskState _state;
        private TaskPriority _priority;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DateTime CreationDate { get; }

        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (value < CreationDate)
                    throw new ArgumentOutOfRangeException(nameof(DueDate),
                        "DueDate cannot be earlier than CreationDate.");
                if (_dueDate == value) return;
                _dueDate = value;
                OnPropertyChanged(nameof(DueDate));
                OnPropertyChanged(nameof(IsOverdue));
            }
        }

        public TaskState State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public TaskPriority Priority
        {
            get => _priority;
            set
            {
                if (_priority == value) return;
                _priority = value;
                OnPropertyChanged(nameof(Priority));
            }
        }

        public bool IsOverdue => DateTime.Now > DueDate;

        public TaskModel(
            string title,
            string description,
            DateTime dueDate,
            TaskState state = TaskState.ToDo,
            TaskPriority priority = TaskPriority.Medium)
        {
            _title = title;
            _description = description;
            CreationDate = DateTime.Now;
            DueDate = dueDate;
            _state = state;
            _priority = priority;
        }

        public override string ToString()
        {
            return $"{Title} [{State}] (Priorytet: {Priority}) – due {DueDate:d}";
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}