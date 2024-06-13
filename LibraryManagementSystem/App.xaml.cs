using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.Views;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            var context = _serviceProvider.GetRequiredService<LibraryContext>();
            var i = new ItemRepository(context);
            var u = new UserRepository(context);
            var l = new LoanRepository(context);
            //var itemRepository = _serviceProvider.GetRequiredService<IRepository<Item>>();
            //var userRepository = _serviceProvider.GetRequiredService<IRepository<User>>();
            //var loanRepository = _serviceProvider.GetRequiredService<IRepository<Loan>>();

            DataInitializer.Initialize(i,u,l);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LibraryContext>(options =>
                options.UseSqlite("Data Source=library.db"));

            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<LoanService>();
            services.AddSingleton<ItemService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }
}