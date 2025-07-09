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
                var itemVm = new TaskItemViewModel(vm.CreatedTask);
                Tasks.Add(itemVm);
                SelectedTask = itemVm;
            }
        }
    }
}