using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using ProjectTracker.ClassLibrary.ServiceInterfaces;
using ProjectTracker.ClassLibrary.Services;
using ProjectTracker.Model;
using ProjectTracker.Model.Models;
using ProjectTracker.ClassLibrary.ViewModels;
using ProjectTracker.ClassLibrary.ViewModels.ControlViewModels;
using ProjectTracker.Views;
using System;
using System.Windows;
using ProjectTracker.ClassLibrary.Factories;
using ProjectTracker.ClassLibrary.ViewModels.Interfaces;
using ProjectTracker.ClassLibrary.ViewModels.PopupViewModels;
using System.Threading;

namespace ProjectTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex = null;

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
            const string appName = "ProjectTracker";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                Application.Current.Shutdown();
            }

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
            services.AddSingleton<ProjectViewModelFactory>();
            services.AddSingleton<ProjectItemsViewModelFactory>();

            // Model Services
            services.AddSingleton<IProjectDataService, ProjectDataService>();
            services.AddSingleton<IBoardDataService, BoardDataService>();
            services.AddSingleton<IGroupDataService, GroupDataService>();
            services.AddSingleton<IIssueDataService, IssueDataService>();
            services.AddSingleton<ITagDataService, TagDataService>();

            // Views
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainView>();
            services.AddSingleton<HomeView>();
            services.AddScoped<ProjectView>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<TabViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ProjectListViewModel>();

            // Popup ViewModels
            services.AddSingleton<ProjectPopupViewModel>();
            services.AddSingleton<BoardPopupViewModel>();
            services.AddSingleton<GroupPopupViewModel>();
            services.AddSingleton<IssuePopupViewModel>();
            services.AddSingleton<TagPopupViewModel>();
            services.AddSingleton<DeletePopupViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
