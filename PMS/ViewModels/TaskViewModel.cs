using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PMS.Core.Models;
using PMS.Helpers;
using PMS.Services;
using PMS.Views;


namespace PMS.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        public ObservableCollection<TaskItemViewModel> Tasks { get; }
            = new ObservableCollection<TaskItemViewModel>();

        public TaskState[] AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>().ToArray();

        public ICommand ShowAddDialogCommand { get; }

        public TaskViewModel()
        {
            var models = TaskStorageService.Load();
            foreach (var m in models)
                Tasks.Add(
                    new TaskItemViewModel(
                        m,
                        removeByIdCallback: RemoveById,
                        editCallback: EditTask));

            ShowAddDialogCommand = new RelayCommand(OpenAddDialog);
        }

        private void OpenAddDialog()
        {
            var vm = new AddTaskViewModel();
            var win = new AddTaskWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (win.ShowDialog() != true || vm.CreatedTask == null)
                return;

            var item = new TaskItemViewModel(
                vm.CreatedTask,
                RemoveById,
                EditTask);

            Tasks.Add(item);
            SaveAll();

        }

        private void RemoveById(Guid id)
        {
            var item = Tasks.FirstOrDefault(t => t.Id == id);
            if (item == null) return;

            var res = MessageBox.Show(
                $"Czy na pewno usunąć zadanie \"{item.Title}\"?",
                "Potwierdź usunięcie",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (res != MessageBoxResult.Yes) return;

            Tasks.Remove(item);
        }

        private void EditTask(TaskItemViewModel item)
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
                item.Title = editVm.CreatedTask.Title;
                item.Description = editVm.CreatedTask.Description;
                item.DueDate = editVm.CreatedTask.DueDate;
                item.State = editVm.CreatedTask.State;
                item.Priority = editVm.CreatedTask.Priority;
            }
        }

        private void SaveAll()
        {
            TaskStorageService.Save(Tasks.Select(t => t.Model));
        }

    }
}