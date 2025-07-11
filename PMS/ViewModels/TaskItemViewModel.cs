using PMS.Core.Models;
using PMS.Helpers;
using System.Windows.Input;

namespace PMS.ViewModels
{
    public class TaskItemViewModel : ViewModelBase
    {
        private readonly TaskModel _model;
        private readonly Action<Guid> _removeByIdCallback;
        private readonly Action<TaskItemViewModel> _editCallback;

        public TaskItemViewModel(
            TaskModel model,
            Action<Guid> removeByIdCallback,
            Action<TaskItemViewModel> editCallback)
        {
            _model = model;
            _removeByIdCallback = removeByIdCallback;
            _editCallback = editCallback;

            RemoveCommand = new RelayCommand(() => _removeByIdCallback(Id));
            EditCommand = new RelayCommand(() => _editCallback(this));
        }

        public TaskModel Model => _model;

        public Guid Id => _model.Id;

        public string Title
        {
            get => _model.Title;
            set
            {
                if (_model.Title != value)
                {
                    _model.Title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => _model.Description;
            set
            {
                if (_model.Description != value)
                {
                    _model.Description = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DueDate
        {
            get => _model.DueDate;
            set
            {
                if (_model.DueDate != value)
                {
                    _model.DueDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public TaskState State
        {
            get => _model.State;
            set
            {
                if (_model.State != value)
                {
                    _model.State = value;
                    OnPropertyChanged();
                }
            }
        }

        public TaskPriority Priority
        {
            get => _model.Priority;
            set
            {
                if (_model.Priority != value)
                {
                    _model.Priority = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RemoveCommand { get; }
        public ICommand EditCommand { get; }
    }
}