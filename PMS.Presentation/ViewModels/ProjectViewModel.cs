using PMS.Domain.Models;
using PMS.Presentation.Common;
using System.Collections.ObjectModel;

namespace PMS.Presentation.ViewModels
{
    public class ProjectViewModel : ViewModelBase
    {
        private readonly Action<TaskViewModel> _onEditTask;
        private readonly Action<TaskViewModel> _onDeleteTask;

        public ProjectModel Model { get; }

        public string Name
        {
            get => Model.Name;
            set
            {
                if (Model.Name == value) return;
                Model.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public DateTimeOffset Deadline
        {
            get => Model.Deadline;
            set
            {
                if (Model.Deadline == value) return;
                Model.Deadline = value;
                RaisePropertyChanged(nameof(Deadline));
            }
        }

        public int State
        {
            get => Model.State;
            set
            {
                if (Model.State == value) return;
                Model.State = value;
                RaisePropertyChanged(nameof(State));
            }
        }

        public ObservableCollection<TaskViewModel> Tasks { get; }

        public ProjectViewModel(
            ProjectModel model,
            Action<TaskViewModel> onEditTask,
            Action<TaskViewModel> onDeleteTask)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            _onEditTask = onEditTask ?? throw new ArgumentNullException(nameof(onEditTask));
            _onDeleteTask = onDeleteTask ?? throw new ArgumentNullException(nameof(onDeleteTask));

            // Inicjalizacja kolekcji VM-owych z już zapisanych TaskModel
            Tasks = new ObservableCollection<TaskViewModel>(
                Model.Tasks.Select(m => new TaskViewModel(m, _onEditTask, _onDeleteTask)));

            // Synchronizacja: wszelkie zmiany w Tasks przenosimy do Model.Tasks
            Tasks.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                    foreach (TaskViewModel vm in e.NewItems)
                        Model.Tasks.Add(vm.Model);

                if (e.OldItems != null)
                    foreach (TaskViewModel vm in e.OldItems)
                        Model.Tasks.Remove(vm.Model);
            };
        }

        // Wywołania delegatów (opcjonalnie, można użyć ICommand)
        public void EditTask(TaskViewModel taskVm) => _onEditTask(taskVm);
        public void DeleteTask(TaskViewModel taskVm) => _onDeleteTask(taskVm);
    }
}