using PMS.Domain.Models;
using PMS.Presentation.Common;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PMS.Presentation.ViewModels
{
    public class AddTaskViewModel : ViewModelBase
    {
        // 1) Backing field i property zamiast pola
        private ObservableCollection<TaskState> _allStates;
        public ObservableCollection<TaskState> AllStates
        {
            get => _allStates;
            private set => SetProperty(ref _allStates, value);
        }

        private ObservableCollection<TaskPriority> _allPriorities;
        public ObservableCollection<TaskPriority> AllPriorities
        {
            get => _allPriorities;
            private set => SetProperty(ref _allPriorities, value);
        }

        // 2) Title z powiadomieniem o zmianie i odświeżeniem CanExecute
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                if (!SetProperty(ref _title, value)) return;

                // odświeżamy dostępność komendy
                (ConfirmCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime _deadline = DateTime.Today;
        public DateTime Deadline
        {
            get => _deadline;
            set => SetProperty(ref _deadline, value);
        }

        private TaskState _state = TaskState.ToDo;
        public TaskState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        private TaskPriority _priority = TaskPriority.Medium;
        public TaskPriority Priority
        {
            get => _priority;
            set => SetProperty(ref _priority, value);
        }

        public event EventHandler<bool>? CloseRequested;

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public AddTaskViewModel()
        {
            AllStates = new ObservableCollection<TaskState>(
                                Enum.GetValues(typeof(TaskState))
                                    .Cast<TaskState>());
            AllPriorities = new ObservableCollection<TaskPriority>(
                                Enum.GetValues(typeof(TaskPriority))
                                    .Cast<TaskPriority>());

            ConfirmCommand = new RelayCommand(_ => OnConfirm(), _ => CanConfirm());
            CancelCommand = new RelayCommand(_ => OnCancel());
        }

        private bool CanConfirm()
            => !string.IsNullOrWhiteSpace(Title);

        private void OnConfirm()
            => CloseRequested?.Invoke(this, true);

        private void OnCancel()
            => CloseRequested?.Invoke(this, false);
    }
}