using PMS.Domain.Models;
using PMS.Presentation.Common;
using PMS.Presentation.Interfaces;
using PMS.Presentation.Sorting;
using System.Collections.ObjectModel;

namespace PMS.Presentation.ViewModels
{
    public class SortOptionItem
    {
        public SortOption Value { get; }
        public string Label { get; }

        public SortOptionItem(SortOption value, string label)
        {
            Value = value;
            Label = label;
        }
    }

    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IProjectStorage _storage;
        private SortOption _selectedSortOption;
        private ProjectViewModel _selectedProject;

        public MainViewModel(
            IDialogService dialogService,
            IProjectStorage storage)
        {
            _dialogService = dialogService;
            _storage = storage;

            // komendy
            AddProjectCommand = new AsyncRelayCommand(
                ExecuteAddProjectAsync);
            AddTaskCommand = new AsyncRelayCommand(
                ExecuteAddTaskAsync,
                () => SelectedProject != null);

            // opcje sortowania
            SortOptions = new ObservableCollection<SortOptionItem>
            {
                new SortOptionItem(SortOption.TitleAsc,    "Tytuł A-Z"),
                new SortOptionItem(SortOption.TitleDesc,   "Tytuł Z-A"),
                new SortOptionItem(SortOption.PriorityAsc, "Priorytet ↑"),
                new SortOptionItem(SortOption.PriorityDesc,"Priorytet ↓"),
                new SortOptionItem(SortOption.DeadlineAsc, "Deadline ↑"),
                new SortOptionItem(SortOption.DeadlineDesc,"Deadline ↓")
            };
            _selectedSortOption = SortOption.TitleAsc;

            // załaduj istniejące projekty
            LoadAllProjectsAsync();
        }

        public ObservableCollection<SortOptionItem> SortOptions { get; }

        public SortOption SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (SetProperty(ref _selectedSortOption, value))
                    ApplySorting();
            }
        }

        public ObservableCollection<ProjectViewModel> Projects { get; }
            = new ObservableCollection<ProjectViewModel>();

        public ProjectViewModel SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (SetProperty(ref _selectedProject, value))
                {
                    (AddTaskCommand as AsyncRelayCommand)?
                        .NotifyCanExecuteChanged();

                    OnPropertyChanged(nameof(Tasks));
                    ApplySorting();
                }
            }
        }

        public ObservableCollection<TaskViewModel> Tasks
            => SelectedProject?.Tasks
               ?? new ObservableCollection<TaskViewModel>();

        public IAsyncRelayCommand AddProjectCommand { get; }
        public IAsyncRelayCommand AddTaskCommand { get; }

        private async void LoadAllProjectsAsync()
        {
            var models = await _storage.LoadAllAsync();
            foreach (var model in models)
            {
                var vm = new ProjectViewModel(
                    model, OnEditTask, OnDeleteTask);
                Projects.Add(vm);
            }
        }

        private async Task ExecuteAddProjectAsync()
        {
            var vm = new AddProjectViewModel();
            if (!await _dialogService.ShowDialogAsync(vm)) return;

            // zapisz nowy projekt do JSON
            var model = new ProjectModel(vm.Name, vm.Deadline);
            await _storage.SaveAsync(model);

            var projectVm = new ProjectViewModel(
                model, OnEditTask, OnDeleteTask);
            Projects.Add(projectVm);
        }

        private async Task ExecuteAddTaskAsync()
        {
            if (SelectedProject == null) return;

            var vm = new AddTaskViewModel();
            if (!await _dialogService.ShowDialogAsync(vm)) return;

            var model = new TaskModel(
                vm.Title,
                vm.Description,
                vm.Deadline,
                vm.State,
                vm.Priority);
            var taskVm = new TaskViewModel(
                model, OnEditTask, OnDeleteTask);

            SelectedProject.Tasks.Add(taskVm);
            OnPropertyChanged(nameof(Tasks));
            SelectedProject.Model.Tasks.Add(model);

            // zaktualizuj plik projektu
            await _storage.SaveAsync(SelectedProject.Model);

            ApplySorting();
        }

        private void OnEditTask(TaskViewModel taskVm)
        {
            // tutaj ewentualna logika edycji...
            // a potem zapisz projekt
            _ = _storage.SaveAsync(SelectedProject.Model);
        }

        private void OnDeleteTask(TaskViewModel taskVm)
        {
            if (SelectedProject?.Tasks.Remove(taskVm) == true)
            {
                // usuń zadanie i zapisz projekt
                _ = _storage.SaveAsync(SelectedProject.Model);
            }
        }

        private void ApplySorting()
        {
            if (SelectedProject == null) return;

            // materializuj kolekcję przed czyszczeniem
            var sorted = TaskSorter
                .Sort(SelectedProject.Tasks, SelectedSortOption)
                .ToList();

            SelectedProject.Tasks.Clear();
            foreach (var t in sorted)
                SelectedProject.Tasks.Add(t);
        }
    }
}