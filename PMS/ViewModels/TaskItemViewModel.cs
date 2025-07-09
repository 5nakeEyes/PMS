using PMS.Core.Models;

namespace PMS.ViewModels
{
    public class TaskItemViewModel : ViewModelBase
    {
        private readonly TaskModel _model;

        public TaskItemViewModel(TaskModel model)
        {
            _model = model;
        }

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
    }
}


