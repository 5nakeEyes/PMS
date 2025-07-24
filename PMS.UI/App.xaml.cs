using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AppWpf = System.Windows.Application;
using PMS.Application.Repositories;
using PMS.Infrastructure.Repositories;
using PMS.Infrastructure.Services;
using PMS.UI.Views;
using PMS.Presentation.ViewModels;
using PMS.Presentation.Interfaces;
using PMS.UI.Services;


namespace PMS.UI
{
    public partial class App : AppWpf
    {
        public static ServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            // Infrastructure
            services.AddSingleton<ITaskStorageService, TaskStorageService>();
            services.AddTransient<ITaskRepository, TaskRepository>();

            // Platform Dialogue Service
            services.AddSingleton<IDialogService, WpfDialogService>();

            // ViewModels
            services.AddTransient<TaskViewModel>();
            services.AddTransient<AddTaskViewModel>();

            // Windows
            services.AddSingleton<MainWindow>();
            services.AddTransient<AddTaskWindow>();

            Services = services.BuildServiceProvider();

            var main = Services.GetRequiredService<MainWindow>();
            main.Show();

            base.OnStartup(e);
        }
    }
}