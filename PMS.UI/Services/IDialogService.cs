using PMS.Models;
using PMS.UI.ViewModels;

namespace PMS.Views.Services
{
    public interface IDialogService
    {
        bool TryAddProject(out string name, out DateTime deadline);
        bool TryEditProject(ProjectItemViewModel vm, out string newName, out DateTime newDeadline);
        bool TryAddTask(out TaskModel taskModel);
    }

}
