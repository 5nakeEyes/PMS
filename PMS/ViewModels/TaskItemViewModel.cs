using PMS.Core.Models;
using PMS.Helpers;
using System.Windows.Input;

namespace PMS.ViewModels
{
    public class TaskItemViewModel : ViewModelBase
    {
        private readonly TaskModel _model;
        private readonly Action<Guid> _removeByIdCallback;

        public TaskItemViewModel(TaskModel model, Action<Guid> removeByIdCallback)
        {
            _model = model;
            _removeByIdCallback = removeByIdCallback;

            RemoveCommand = new RelayCommand(() => _removeByIdCallback(Id));
        }

        public Guid Id => _model.Id;
        public string Title => _model.Title;
        public string Description => _model.Description;
        public DateTime DueDate => _model.DueDate;
        public TaskPriority Priority => _model.Priority;

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

        public ICommand RemoveCommand { get; }
    }
}


