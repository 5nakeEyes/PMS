using PMS.Helpers;
using PMS.Core.Models;
using PMS.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;


namespace PMS.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        public ObservableCollection<TaskItemViewModel> Tasks { get; } 
            = new ObservableCollection<TaskItemViewModel>();

        private TaskItemViewModel? _selectedTask;
        public TaskItemViewModel? SelectedTask
        {
            get => _selectedTask;
            set => SetProperty(ref _selectedTask, value);
        }

        public TaskState[] AllStates =>
            Enum.GetValues(typeof(TaskState)).Cast<TaskState>().ToArray();

        public ICommand ShowAddDialogCommand { get; }
        public ICommand RemoveTaskByIdCommand { get; }  // nowa komenda


        public TaskViewModel()
        {
            ShowAddDialogCommand = new RelayCommand(OpenAddDialog);
        }

        private void OpenAddDialog()
        {
            var vm = new AddTaskViewModel();
            var win = new AddTaskWindow(vm) { Owner = Application.Current.MainWindow };

            if (win.ShowDialog() == true && vm.CreatedTask != null)
            {
                // przekazujemy TaskItemViewModelowi referencję do RemoveById
                var itemVm = new TaskItemViewModel(vm.CreatedTask, RemoveById);
                Tasks.Add(itemVm);
            }
        }

        private void RemoveById(Guid id)
        {
            var item = Tasks.FirstOrDefault(t => t.Id == id);
            if (item == null) return;

            var res = MessageBox.Show(
                $"Czy na pewno usunąć zadanie “{item.Title}”?",
                "Potwierdź",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (res != MessageBoxResult.Yes) return;

            Tasks.Remove(item);
        }
    }
}