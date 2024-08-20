using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Repositories;
using LibraryManagementSystem.ViewModels;
using LibraryManagementSystem.Views;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.Extensions.Logging;

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
            context.Database.Migrate();

            var itemRepository = _serviceProvider.GetRequiredService<IRepository<Item>>();
            var userRepository = _serviceProvider.GetRequiredService<IRepository<User>>();
            var loanRepository = _serviceProvider.GetRequiredService<IRepository<Loan>>();
            var reviewRepository = _serviceProvider.GetRequiredService<IRepository<Review>>();
            var fineRepository = _serviceProvider.GetRequiredService<IRepository<Fine>>();
            var fineRequestRepository = _serviceProvider.GetRequiredService<IRepository<FinePayRequest>>();
            var loanRequestRepository = _serviceProvider.GetRequiredService<IRepository<LoanRequest>>();

            // Initialize the data
            DataInitializer.Initialize(itemRepository, userRepository, loanRepository, reviewRepository, fineRepository, loanRequestRepository, fineRequestRepository);

            // Generate or update fines at startup
            var fineService = _serviceProvider.GetRequiredService<FineService>();
            fineService.GenerateOrUpdateFines();

            // Show the main window
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LibraryContext>(options =>
                options.UseSqlite(@"Data Source=C:\Users\ItayTzafrir\source\repos\LibraryManagementSystem\LibraryManagementSystem\library.db")
                    .LogTo(Console.WriteLine, LogLevel.Information));

            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<ItemService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<FineService>(); // Register FineService
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();

            // Register the repositories
            services.AddSingleton<IRepository<Item>, Repository<Item>>();
            services.AddSingleton<IRepository<User>, Repository<User>>();
            services.AddSingleton<IRepository<Loan>, Repository<Loan>>();
            services.AddSingleton<IRepository<Review>, Repository<Review>>();
            services.AddSingleton<IRepository<Fine>, Repository<Fine>>();
            services.AddSingleton<IRepository<LoanRequest>, Repository<LoanRequest>>();
            services.AddSingleton<IRepository<FinePayRequest>, Repository<FinePayRequest>>();
        }
    }
}