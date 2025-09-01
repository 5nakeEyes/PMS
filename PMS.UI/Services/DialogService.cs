using PMS.Models;
using PMS.UI.ViewModels;
using PMS.UI.Views;
using System.Windows;

namespace PMS.Views.Services
{
    public sealed class DialogService : IDialogService
    {
        public bool TryAddProject(out string name, out DateTime deadline)
        {
            var vm = new AddProjectViewModel();
            var dlg = new AddProjectWindow { Owner = Application.Current.MainWindow, DataContext = vm, Title="Add project" };
            if (dlg.ShowDialog() == true)
            {
                name = vm.Name?.Trim();
                deadline = vm.Deadline!.Value;
                return true;
            }
            name = default; deadline = default;
            return false;
        }

        public bool TryEditProject(ProjectItemViewModel pvm, out string newName, out DateTime newDeadline)
        {
            var vm = new AddProjectViewModel { Name = pvm.Name, Deadline = pvm.Deadline };
            var dlg = new AddProjectWindow { Owner = Application.Current.MainWindow, DataContext = vm, Title = "Edit project" };
            if (dlg.ShowDialog() == true)
            {
                newName = vm.Name!.Trim();
                newDeadline = vm.Deadline!.Value;
                return true;
            }
            newName = default; newDeadline = default;
            return false;
        }

        public bool TryAddTask(out TaskModel taskModel)
        {
            var vm = new AddTaskViewModel();
            var dlg = new AddTaskWindow { Owner = Application.Current.MainWindow, DataContext = vm, Title = "Add task" };
            if (dlg.ShowDialog() == true)
            {
                taskModel = vm.ToTaskModel();
                return true;
            }
            taskModel = default!;
            return false;
        }

        public bool ShowConfirmation(string message)
        {
            var vm = new ConfirmationDialogViewModel(message);
            var view = new ConfirmationDialog
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow
            };
            return view.ShowDialog() == true;
        }

    }
}
