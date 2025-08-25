using PMS.Views.Services;
using PMS.Views.ViewModels;

namespace PMS.UI.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        public ProjectsViewModel ProjectList { get; }
        public TasksViewModel TaskList { get; }

        public MainViewModel()
            : this(new JsonProjectRepository(), new DialogService()) { }

        public MainViewModel(IProjectRepository repo, IDialogService dialogs)
        {
            ProjectList = new ProjectsViewModel(repo, dialogs);
            TaskList = new TasksViewModel(repo, dialogs);

            ProjectList.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ProjectsViewModel.SelectedProject))
                    TaskList.Project = ProjectList.SelectedProject;
            };

            TaskList.Project = ProjectList.SelectedProject;
        }
    }
}