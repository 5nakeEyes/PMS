using PMS.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PMS.UI.ViewModels
{
    public class ProjectItemViewModel : ViewModelBase
    {
        private readonly ProjectModel _model;
        public ProjectModel Model => _model;

        public string Name
        {
            get => _model.Name;
            set
            {
                if (_model.Name == value) return;
                _model.Name = value;
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

        public ObservableCollection<TaskItemViewModel> Tasks { get; }

        public ProjectItemViewModel(ProjectModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

            Tasks = new ObservableCollection<TaskItemViewModel>(
                model.Tasks.Select(t => new TaskItemViewModel(t))
            );

            Tasks.CollectionChanged += Tasks_CollectionChanged;
            foreach (var t in Tasks)
                t.PropertyChanged += Task_PropertyChanged;
        }

        private void Tasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (TaskItemViewModel t in e.NewItems)
                {
                    t.PropertyChanged += Task_PropertyChanged;
                    _model.Tasks.Add(t.Model);
                }

            if (e.OldItems != null)
                foreach (TaskItemViewModel t in e.OldItems)
                {
                    t.PropertyChanged -= Task_PropertyChanged;
                    _model.Tasks.Remove(t.Model);
                }

            RaiseStatsChanged();
        }

        private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TaskItemViewModel.State))
                RaiseStatsChanged();
        }

        private void RaiseStatsChanged()
        {
            OnPropertyChanged(nameof(CompletedCount));
            OnPropertyChanged(nameof(TotalCount));
            OnPropertyChanged(nameof(ProgressText));
            OnPropertyChanged(nameof(OverallState));
        }

        public int CompletedCount => Tasks.Count(t => t.State == TaskState.Done);

        public int TotalCount => Tasks.Count;

        public string ProgressText => $"{CompletedCount}/{TotalCount}";

        public string OverallState
        {
            get
            {
                if (TotalCount == 0 || Tasks.All(t => t.State == TaskState.ToDo))
                    return "NotStarted";
                if (Tasks.All(t => t.State == TaskState.Done))
                    return "Completed";
                return "InProgress";
            }
        }
    }
}