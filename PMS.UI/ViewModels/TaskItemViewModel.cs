using PMS.Commands;
using PMS.Models;
using PMS.UI.Views;
using System.Windows;
using System.Windows.Input;

namespace PMS.UI.ViewModels
{
    public class TaskItemViewModel : ViewModelBase
    {
        private readonly TaskModel _model;
        public TaskModel Model => _model;

        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ShowPreviewCommand { get; }

        public event Action<TaskItemViewModel> DeleteRequested;

        public string Title
        {
            get => _model.Title;
            set
            {
                if (_model.Title == value) return;
                _model.Title = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _model.Description;
            set
            {
                if (_model.Description == value) return;
                _model.Description = value;
                OnPropertyChanged();
            }
        }

        public DateTime Deadline
        {
            get => _model.Deadline;
            set
            {
                if (_model.Deadline == value) return;
                _model.Deadline = value;
                OnPropertyChanged();
            }
        }

        public TaskState State
        {
            get => _model.State;
            set
            {
                if (_model.State == value) return;
                _model.State = value;
                OnPropertyChanged();
            }
        }

        public TaskPriority Priority
        {
            get => _model.Priority;
            set
            {
                if (_model.Priority == value) return;
                _model.Priority = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<TaskState> AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>();

        public IEnumerable<TaskPriority> AllPriorities =>
            Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>();

        public TaskItemViewModel(TaskModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            DeleteCommand = new RelayCommand(_ =>
            {
                DeleteRequested?.Invoke(this);
            });

            EditCommand = new RelayCommand(_ => EditTask());

            ShowPreviewCommand = new RelayCommand(_ => ShowTaskPreview());
        }

        private void EditTask()
        {
            var editVm = new AddTaskViewModel
            {
                Title = this.Title,
                Description = this.Description,
                Deadline = this.Deadline,
                State = this.State,
                Priority = this.Priority
            };

            var window = new AddTaskDialog
            {
                Owner = Application.Current.MainWindow,
                DataContext = editVm,
                Title = "Edit task"
            };

            if (window.ShowDialog() == true)
            {
                Title = editVm.Title;
                Description = editVm.Description;
                Deadline = editVm.Deadline.Value;
                State = editVm.State;
                Priority = editVm.Priority;
            }
        }

        private void ShowTaskPreview()
        {
            var previewVm = new PreviewTaskViewModel(_model);

            var window = new PreviewTaskDialog
            {
                Owner = Application.Current.MainWindow,
                DataContext = previewVm,
                Title = "Preview task"
            };
            window.ShowDialog();
        }
    }
}