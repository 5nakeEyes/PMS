using PMS.Models;

namespace PMS.UI.ViewModels
{
    public class PreviewTaskViewModel : ViewModelBase
    {
        private readonly TaskModel _model;
        public TaskModel Model => _model;

        public string Title
        {
            get => _model.Title;
        }

        public string Description
        {
            get => _model.Description;
        }

        public DateTime Deadline
        {
            get => _model.Deadline;
        }

        public TaskState State
        {
            get => _model.State;
        }

        public TaskPriority Priority
        {
            get => _model.Priority;
        }

        public IEnumerable<TaskState> AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>();

        public IEnumerable<TaskPriority> AllPriorities =>
            Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>();

        public PreviewTaskViewModel(TaskModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }
    }
}
