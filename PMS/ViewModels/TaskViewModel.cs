using PMS.Helpers;
using PMS.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PMS.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        public ObservableCollection<TaskModel> Tasks { get; }
            = new ObservableCollection<TaskModel>();

        private TaskModel? _selectedTask;
        public TaskModel? SelectedTask
        {
            get => _selectedTask;
            set => SetProperty(ref _selectedTask, value);
        }

        public ICommand ShowAddDialogCommand { get; }

        public TaskViewModel()
        {
            ShowAddDialogCommand = new RelayCommand(OpenAddDialog);
        }

        private void OpenAddDialog()
        {
            var vm = new AddTaskViewModel();
            var win = new AddTaskWindow(vm)
            {
                Owner = Application.Current.MainWindow
            };

            if (win.ShowDialog() == true && vm.CreatedTask != null)
            {
                Tasks.Add(vm.CreatedTask);
                SelectedTask = vm.CreatedTask;
            }
        }
    }
}