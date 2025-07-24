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

        public IAsyncCommand AddCommand { get; }

        public TaskViewModel(
            ITaskRepository repo,
            IDialogService dialogs)
        {
            _repo = repo;
            _dialogs = dialogs;

            AddCommand = new AsyncCommand(_ => OpenAddAsync());

            _ = LoadAsync();
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
        }

        private async Task OpenAddAsync()
        {
            var vm = new AddTaskViewModel(_repo, _dialogs);
            var result = await _dialogs.ShowDialogAsync(vm);
            if (result && vm.CreatedTask is TaskModel created)
            {
                var itemVm = new TaskItemViewModel(created, _repo, _dialogs);
                itemVm.Deleted += vm => Tasks.Remove(vm);
                Tasks.Add(itemVm);
            }
        }
    }
}