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

        public ObservableCollection<TaskItemViewModel> Tasks { get; }
            = new ObservableCollection<TaskItemViewModel>();

        // 1. Opcje sortowania
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
        // 2. Właściwość wyzwalająca sortowanie
        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                if (SetProperty(ref _selectedSortOption, value))
                    ApplySorting();
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
            _ = LoadAsync();
        }

        private void ApplySorting()
        {
            IEnumerable<TaskItemViewModel> sorted = SelectedSortOption switch
            {
                "Tytuł A-Z" => Tasks.OrderBy(t => t.Title),
                "Tytuł Z-A" => Tasks.OrderByDescending(t => t.Title),
                "Priorytet ↑" => Tasks.OrderBy(t => t.Priority),
                "Priorytet ↓" => Tasks.OrderByDescending(t => t.Priority),
                "Data newest" => Tasks.OrderByDescending(t => t.StartDate),
                "Data oldest" => Tasks.OrderBy(t => t.StartDate),
                _ => Tasks
            };

            // Przebuduj ObservableCollection
            var list = sorted.ToList();
            Tasks.Clear();
            foreach (var vm in list)
                Tasks.Add(vm);
        }

        private async Task LoadAsync()
        {
            Tasks.Clear();
            var models = await _repo.GetAllAsync();
            foreach (var m in models)
            {
                var itemVm = new TaskItemViewModel(m, _repo, _dialogs);
                itemVm.Deleted += vm => Tasks.Remove(vm);
                Tasks.Add(itemVm);
            }

            // Po załadowaniu danych od razu sortujemy
            ApplySorting();
        }

        private async Task OpenAddAsync()
        {
            var vm = new AddTaskViewModel(_repo, _dialogs);
            var ok = await _dialogs.ShowDialogAsync(vm);
            if (!ok || vm.CreatedTask is not TaskModel created) return;

            var itemVm = new TaskItemViewModel(created, _repo, _dialogs);
            itemVm.Deleted += vm => Tasks.Remove(vm);
            Tasks.Add(itemVm);

            ApplySorting();
        }
    }
}