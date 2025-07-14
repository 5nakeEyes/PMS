using PMS.Core.Models;
using PMS.Helpers;
using PMS.Services;
using PMS.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PMS.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        private readonly ITaskStorageService _storage;

        public ObservableCollection<TaskItemViewModel> Tasks { get; }
            = new ObservableCollection<TaskItemViewModel>();

        public TaskState[] AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>().ToArray();

        public ICommand ShowAddDialogCommand { get; }

        public TaskViewModel(ITaskStorageService storage)
        {
            _storage = storage;
            ShowAddDialogCommand = new RelayCommand(OpenAddDialog);
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            var models = await _storage.LoadAsync();
            foreach (var m in models)
                Tasks.Add(CreateItemVm(m));
        }

        private TaskItemViewModel CreateItemVm(TaskModel model)
            => new TaskItemViewModel(
                   model,
                   removeByIdCallback: async id =>
                   {
                       var vm = Tasks.FirstOrDefault(t => t.Id == id);
                       if (vm == null) return;

                       var res = MessageBox.Show(
                           $"Czy na pewno usunąć zadanie \"{vm.Title}\"?",
                           "Potwierdź usunięcie",
                           MessageBoxButton.YesNo,
                           MessageBoxImage.Warning);

                       if (res != MessageBoxResult.Yes) return;

                       Tasks.Remove(vm);
                       await SaveAllAsync();
                   },
                   editCallback: async item =>
                   {
                       var editVm = new AddTaskViewModel
                       {
                           Title = item.Title,
                           Description = item.Description,
                           DueDate = item.DueDate,
                           State = item.State,
                           Priority = item.Priority
                       };
                       var win = new AddTaskWindow(editVm)
                       {
                           Owner = Application.Current.MainWindow,
                           Title = "Edytuj zadanie"
                       };

                       if (win.ShowDialog() == true && editVm.CreatedTask != null)
                       {
                           var t = editVm.CreatedTask;
                           item.Title       = t.Title;
                           item.Description = t.Description;
                           item.DueDate     = t.DueDate;
                           item.State       = t.State;
                           item.Priority    = t.Priority;

                           await SaveAllAsync();
                       }
                   });

        private void OpenAddDialog()
        {
            var vm = new AddTaskViewModel();
            var win = new AddTaskWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (win.ShowDialog() == true && vm.CreatedTask != null)
            {
                var item = CreateItemVm(vm.CreatedTask);
                Tasks.Add(item);
                _ = SaveAllAsync();
            }
        }

        private async Task SaveAllAsync()
        {
            var models = Tasks.Select(t => t.Model).ToList();
            await _storage.SaveAsync(models);
        }
    }
}