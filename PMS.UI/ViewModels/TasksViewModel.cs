using PMS.Commands;
using PMS.UI.ViewModels;
using PMS.Views.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace PMS.Views.ViewModels
{
    public sealed class TasksViewModel : ViewModelBase
    {
        private readonly IProjectRepository _repo;
        private readonly IDialogService _dialogs;

        public ReadOnlyObservableCollection<string> SortOptions { get; }
        private static readonly string[] _sortOptions =
        {
        "Title A-Z", "Title Z-A", "Priority ↑", "Priority ↓", "Deadline"
    };

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set { if (SetProperty(ref _selectedSortOption, value)) ApplyTaskSorting(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { if (SetProperty(ref _searchText, value)) _tasksCvs.View?.Refresh(); }
        }
        public ICommand ClearSearchCommand { get; }

        private ProjectItemViewModel _project;
        public ProjectItemViewModel Project
        {
            get => _project;
            set
            {
                if (SetProperty(ref _project, value))
                    HookTasks(_project);
            }
        }

        public ICommand AddTaskCommand { get; }
        public ICollectionView TasksView => _tasksCvs.View;

        private readonly CollectionViewSource _tasksCvs = new();
        private ObservableCollection<TaskItemViewModel> _currentTasks;

        public TasksViewModel(IProjectRepository repo, IDialogService dialogs)
        {
            _repo = repo;
            _dialogs = dialogs;

            SortOptions = new ReadOnlyObservableCollection<string>(new ObservableCollection<string>(_sortOptions));
            _selectedSortOption = _sortOptions[4];

            _tasksCvs.Filter += TaskFilter;
            ClearSearchCommand = new RelayCommand(_ => SearchText = string.Empty);
            AddTaskCommand = new RelayCommand(_ => AddTask(), _ => Project != null);
        }

        private void HookTasks(ProjectItemViewModel project)
        {
            // unhook old
            if (_currentTasks != null)
            {
                _currentTasks.CollectionChanged -= Tasks_CollectionChanged;
                foreach (var t in _currentTasks)
                {
                    t.PropertyChanged -= Task_PropertyChanged;
                    t.DeleteRequested -= Task_DeleteRequested;
                }
            }

            _currentTasks = project?.Tasks;
            _tasksCvs.Source = _currentTasks ?? Enumerable.Empty<TaskItemViewModel>();
            OnPropertyChanged(nameof(TasksView));

            // hook new
            if (_currentTasks != null)
            {
                _currentTasks.CollectionChanged += Tasks_CollectionChanged;
                foreach (var t in _currentTasks)
                {
                    t.PropertyChanged += Task_PropertyChanged;
                    t.DeleteRequested += Task_DeleteRequested;
                }
            }

            ApplyTaskSorting();
        }

        private void Task_DeleteRequested(TaskItemViewModel vm)
        {
            bool confirm = _dialogs.ShowConfirmation(
            $"Delete task „{vm.Title}”?");

            if (!confirm)
                return;

            if (_currentTasks.Contains(vm))
            {
                _currentTasks.Remove(vm);
                if (Project != null)
                    _repo.Save(Project.Model);
            }
        }

        private void Tasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (TaskItemViewModel t in e.NewItems)
                {
                    t.PropertyChanged += Task_PropertyChanged;
                    t.DeleteRequested += Task_DeleteRequested;
                }
            if (e.OldItems != null)
                foreach (TaskItemViewModel t in e.OldItems)
                {
                    t.PropertyChanged -= Task_PropertyChanged;
                    t.DeleteRequested -= Task_DeleteRequested;
                }
            if (Project != null) _repo.Save(Project.Model);
        }

        private void Task_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (Project != null) _repo.Save(Project.Model);

            if (e.PropertyName is nameof(TaskItemViewModel.Priority)
                or nameof(TaskItemViewModel.Title)
                or nameof(TaskItemViewModel.Deadline))
            {
                ApplyTaskSorting();
            }
        }

        private void TaskFilter(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_searchText)) { e.Accepted = true; return; }
            var task = (TaskItemViewModel)e.Item;
            var text = _searchText.Trim();

            bool matchesTitle = task.Title?.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
            bool matchesDescription = task.Description?.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
            bool matchesDeadline = task.Deadline.ToString().IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
            bool matchesState = task.State.ToString().IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
            bool matchesPriority = task.Priority.ToString().IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;

            e.Accepted = matchesTitle || matchesDescription || matchesDeadline || matchesState || matchesPriority;
        }

        private void ApplyTaskSorting()
        {
            var view = _tasksCvs.View;
            if (view == null) return;

            view.SortDescriptions.Clear();
            switch (SelectedSortOption)
            {
                case "Title A-Z":
                    view.SortDescriptions.Add(new SortDescription(nameof(TaskItemViewModel.Title), ListSortDirection.Ascending)); break;
                case "Title Z-A":
                    view.SortDescriptions.Add(new SortDescription(nameof(TaskItemViewModel.Title), ListSortDirection.Descending)); break;
                case "Priority ↑":
                    view.SortDescriptions.Add(new SortDescription(nameof(TaskItemViewModel.Priority), ListSortDirection.Ascending)); break;
                case "Priority ↓":
                    view.SortDescriptions.Add(new SortDescription(nameof(TaskItemViewModel.Priority), ListSortDirection.Descending)); break;
                case "Deadline":
                    view.SortDescriptions.Add(new SortDescription(nameof(TaskItemViewModel.Deadline), ListSortDirection.Ascending)); break;
            }
            view.Refresh();
        }

        private void AddTask()
        {
            if (Project == null) return;
            if (!_dialogs.TryAddTask(out var taskModel)) return;

            Project.Tasks.Add(new TaskItemViewModel(taskModel));
            _repo.Save(Project.Model);
        }
    }

}
