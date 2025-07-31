using PMS.Application.Repositories;
using PMS.Domain.Models;
using PMS.Presentation.Common;
using PMS.Presentation.Interfaces;
using System.Collections.ObjectModel;

namespace PMS.Presentation.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private readonly ITaskRepository _repo;
        private readonly IDialogService _dialogs;

        // 1) Bufor wszystkich VM-ek
        private readonly List<TaskItemViewModel> _allTasks = new List<TaskItemViewModel>();

        // 2) Kolekcja widoczna w UI
        public ObservableCollection<TaskItemViewModel> Tasks { get; }
            = new ObservableCollection<TaskItemViewModel>();

        // 3) Opcje sortowania
        public ObservableCollection<string> SortOptions { get; }
            = new ObservableCollection<string>
        {
            "Tytuł A-Z",
            "Tytuł Z-A",
            "Priorytet ↑",
            "Priorytet ↓",
            "Data newest",
            "Data oldest"
        };

        private string _selectedSortOption;
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (SetProperty(ref _selectedSortOption, value))
                    RefreshTasks();
            }
        }

        // 4) Tekst wyszukiwania
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    RefreshTasks();
            }
        }

        public IAsyncCommand AddCommand { get; }

        public TaskViewModel(
            ITaskRepository repo,
            IDialogService dialogs)
        {
            _repo = repo;
            _dialogs = dialogs;

            AddCommand = new AsyncCommand(_ => OpenAddAsync());

            SelectedSortOption = SortOptions.First();
            SearchText = string.Empty;
            _ = LoadAsync();
        }

        // 5) Łączy filtr i sortowanie, potem odświeża Tasks
        private void RefreshTasks()
        {
            // filtruj po tytule
            var filtered = _allTasks
                .Where(vm => string.IsNullOrWhiteSpace(SearchText)
                             || vm.Title.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase));

            // zastosuj sortowanie
            var sorted = SelectedSortOption switch
            {
                "Tytuł A-Z" => filtered.OrderBy(t => t.Title),
                "Tytuł Z-A" => filtered.OrderByDescending(t => t.Title),
                "Priorytet ↑" => filtered.OrderBy(t => t.Priority),
                "Priorytet ↓" => filtered.OrderByDescending(t => t.Priority),
                "Data newest" => filtered.OrderByDescending(t => t.StartDate),
                "Data oldest" => filtered.OrderBy(t => t.StartDate),
                _ => filtered
            };

            // odśwież widoczną kolekcję
            Tasks.Clear();
            foreach (var vm in sorted)
                Tasks.Add(vm);
        }

        private async Task LoadAsync()
        {
            _allTasks.Clear();
            Tasks.Clear();

            var models = await _repo.GetAllAsync();
            foreach (var m in models)
            {
                var vm = new TaskItemViewModel(m, _repo, _dialogs);
                _allTasks.Add(vm);
            }

            RefreshTasks();
        }

        private async Task OpenAddAsync()
        {
            var addVm = new AddTaskViewModel(_repo, _dialogs);
            var ok = await _dialogs.ShowDialogAsync(addVm);
            if (!ok || addVm.CreatedTask is not TaskModel created)
                return;

            var itemVm = new TaskItemViewModel(created, _repo, _dialogs);
            _allTasks.Add(itemVm);

            RefreshTasks();
        }
    }
}