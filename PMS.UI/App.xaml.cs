using Microsoft.Extensions.DependencyInjection;
using PMS.UI.Views;
using System.Windows;

namespace PMS.UI
{
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            // validateScopes: true – wykryje błędy w rejestracji scoped → singleton
            ServiceProvider = services.BuildServiceProvider(validateScopes: true);

            // uruchamiamy główne okno przez DI
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // upewniamy się, że kontener się zdispose’uje
            if (ServiceProvider is IDisposable disp)
            {
                disp.Dispose();
            }

            base.OnExit(e);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // ---- Infrastructure ----

            // ---- Platformowe serwisy ----

            // ---- ViewModels ----

            // ---- Widoki ----
            services.AddSingleton<MainWindow>();
            services.AddTransient<AddTaskWindow>();
            services.AddTransient<AddProjectWindow>();
        }
    }
}