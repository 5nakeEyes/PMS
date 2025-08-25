using PMS.Commands;
using PMS.Models;
using PMS.UI.ViewModels;
using PMS.Views.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PMS.Views.ViewModels
{
    public sealed class ProjectsViewModel : ViewModelBase
    {
        private readonly IProjectRepository _repo;
        private readonly IDialogService _dialogs;

        public ObservableCollection<ProjectItemViewModel> Projects { get; } = new();
        private ProjectItemViewModel _selectedProject;
        public ProjectItemViewModel SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }

        public ICommand AddProjectCommand { get; }
        public ICommand RemoveProjectCommand { get; }
        public ICommand EditProjectCommand { get; }
        public ICommand ReloadCommand { get; }
        public ICommand SaveAllCommand { get; }

        public ProjectsViewModel(IProjectRepository repo, IDialogService dialogs)
        {
            _repo = repo;
            _dialogs = dialogs;

            AddProjectCommand = new RelayCommand(_ => AddProject());
            RemoveProjectCommand = new RelayCommand(_ => RemoveProject(), _ => SelectedProject != null);
            EditProjectCommand = new RelayCommand(_ => EditProject(), _ => SelectedProject != null);
            ReloadCommand = new RelayCommand(_ => Reload());
            SaveAllCommand = new RelayCommand(_ => SaveAll(), _ => Projects.Any());

            Reload();
        }

        private void AddProject()
        {
            if (!_dialogs.TryAddProject(out var name, out var deadline)) return;

            var model = new ProjectModel(name, deadline);
            _repo.Save(model);
            var vm = new ProjectItemViewModel(model);
            Projects.Add(vm);
            SelectedProject = vm;
        }

        private void RemoveProject()
        {
            if (SelectedProject == null) return;

            _repo.Delete(SelectedProject.Model.Name);
            Projects.Remove(SelectedProject);
            SelectedProject = Projects.FirstOrDefault();
        }

        private void EditProject()
        {
            if (SelectedProject == null) return;

            var oldName = SelectedProject.Name;
            if (!_dialogs.TryEditProject(SelectedProject, out var newName, out var newDeadline)) return;

            if (!string.Equals(oldName, newName, StringComparison.Ordinal))
            {
                _repo.Delete(oldName);
                SelectedProject.Name = newName;
                SelectedProject.Model.Name = newName;
            }

            SelectedProject.Deadline = newDeadline;
            SelectedProject.Model.Deadline = newDeadline;

            _repo.Save(SelectedProject.Model);
        }

        private void Reload()
        {
            Projects.Clear();
            foreach (var name in _repo.GetAllProjectNames())
            {
                try
                {
                    var model = _repo.Load(name);
                    Projects.Add(new ProjectItemViewModel(model));
                }
                catch { /* log */ }
            }
            SelectedProject = Projects.FirstOrDefault();
        }

        private void SaveAll()
        {
            foreach (var p in Projects)
                _repo.Save(p.Model);
        }
    }

}
