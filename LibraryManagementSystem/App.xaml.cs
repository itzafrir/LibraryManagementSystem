﻿using Microsoft.Extensions.DependencyInjection;
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

            DataInitializer.Initialize(itemRepository, userRepository, loanRepository);

            #region Clear DB
            //var entityTypes = context.Model.GetEntityTypes();

            //foreach (var entityType in entityTypes)
            //{
            //    var clrType = entityType.ClrType;

            //    // Get the DbSet for each entity type using reflection
            //    var method = context.GetType().GetMethod("Set", new Type[] { }).MakeGenericMethod(clrType);
            //    var dbSet = method.Invoke(context, null);

            //    // Get the entities in the DbSet
            //    var entities = (dbSet as IQueryable<object>).ToList();

            //    // Remove all entities
            //    context.RemoveRange(entities);
            //}

            //context.SaveChanges();

            #endregion

            // Get list of tables
            var tables = context.Model.GetEntityTypes()
                .Select(t => t.GetTableName())
                .Distinct()
                .ToList();
            string tableList = string.Join(", ", tables);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LibraryContext>(options =>
                options.UseSqlite(@"Data Source=C:\Users\ItayTzafrir\source\repos\LibraryManagementSystem\LibraryManagementSystem\library.db")
                    .LogTo(Console.WriteLine, LogLevel.Information));

            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<LoanService>();
            services.AddSingleton<ItemService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }
}
