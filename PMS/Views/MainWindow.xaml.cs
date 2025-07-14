using Microsoft.Extensions.DependencyInjection;
using PMS.ViewModels;
using System.Windows;

namespace PMS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        : this(App.Services.GetRequiredService<TaskViewModel>())
        {
        }

        public MainWindow(TaskViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}