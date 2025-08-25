using PMS.Models;
using PMS.UI.ViewModels;
using System.Windows.Controls;

namespace PMS.UI.Views
{
    public partial class TaskView : UserControl
    {
        public TaskView()
        {
            InitializeComponent();
        }
        // dodatkowy konstruktor do wstrzykiwania modelu
        public TaskView(TaskModel taskModel) : this()
        {
            DataContext = new TaskItemViewModel(taskModel);
        }

    }
}