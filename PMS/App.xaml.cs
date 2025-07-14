using Microsoft.Extensions.DependencyInjection;
using PMS.Services;
using PMS.ViewModels;
using System.Windows;

namespace PMS
{
    public partial class App : Application
    {
        public static ServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddSingleton<ITaskStorageService, TaskStorageService>();
            services.AddSingleton<MainWindow>();
            services.AddTransient<TaskViewModel>();
            services.AddTransient<AddTaskViewModel>();

            Services = services.BuildServiceProvider();

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}