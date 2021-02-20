using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.Services;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using ProjectTracker.ClassLibrary.ViewModels;
using ProjectTracker.Views;
using System;
using System.Windows;

namespace ProjectTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public readonly IDesignTimeDbContextFactory<ProjectTrackerDBContext> contextFactory = new ProjectTrackerDBContextFactory();
        public static IServiceProvider ServiceProvider { get; set; }

        public App()
        {
            using (ProjectTrackerDBContext context = contextFactory.CreateDbContext(null))
            {
                context.Database.Migrate();
            }

            ServiceProvider = CreateServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Loaded += MainWindow_Loaded;
            mainWindow.Show();
        }

        /// TODO: There might need to be stuff here like signin if not signed in or some other shit
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // Factories
            services.AddSingleton<IDesignTimeDbContextFactory<ProjectTrackerDBContext>, ProjectTrackerDBContextFactory>();

            // Model Services
            services.AddSingleton<IDataService<Project>, GenericDataService<Project>>();

            // Views
            services.AddSingleton<MainWindow>();
            services.AddSingleton<HomeView>();
            services.AddSingleton<ProjectListView>();
            services.AddSingleton<ProjectView>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ProjectListViewModel>();
            services.AddSingleton<ProjectViewModel>();

            // Control ViewModels

            return services.BuildServiceProvider();
        }
    }
}
