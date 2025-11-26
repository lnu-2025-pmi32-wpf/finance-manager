namespace FinanceManager.UI
{
    // <copyright file="App.xaml.cs" company="LNU">
    // Copyright (c) LNU. All rights reserved.
    // </copyright>

    using System;
    using System.IO;
    using System.Windows;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using FinanceManager.Data;
    using FinanceManager.BLL.Services;
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Global exception handlers for easier debugging of startup failures
            AppDomain.CurrentDomain.UnhandledException += (s, ev) =>
            {
                LogAndShowException(ev.ExceptionObject as Exception, "AppDomain.CurrentDomain.UnhandledException");
            };

            this.DispatcherUnhandledException += (s, ev) =>
            {
                LogAndShowException(ev.Exception, "Application.DispatcherUnhandledException");
                ev.Handled = true;
                Current.Shutdown();
            };

            try
            {
                var services = new ServiceCollection();

                // Configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                services.AddSingleton<IConfiguration>(configuration);

                // DbContext
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

                // BLL - register services
                services.AddScoped<IFinancialProfileService, FinancialProfileService>();
                services.AddScoped<IAccountService, FinanceManager.BLL.Services.AccountService>();
                services.AddScoped<ITransactionService, FinanceManager.BLL.Services.TransactionService>();
                services.AddScoped<ICategoryService, FinanceManager.BLL.Services.CategoryService>();

                // UI
                services.AddTransient<MainWindow>();
                services.AddTransient<Views.DashboardView>();
                services.AddScoped<ViewModels.DashboardViewModel>();
                services.AddTransient<Views.CategoriesView>();
                services.AddScoped<ViewModels.CategoriesViewModel>();
                services.AddTransient<Views.AccountsView>();
                services.AddScoped<ViewModels.AccountsViewModel>();
                services.AddTransient<Views.TransactionsView>();
                services.AddScoped<ViewModels.TransactionsViewModel>();

                this.serviceProvider = services.BuildServiceProvider();

                // Seed DB in development (uses DAL DataSeeder)
                using (var scope = this.serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    FinanceManager.Data.DataSeeder.Seed(db);
                }

                var mainWindow = this.serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                LogAndShowException(ex, "OnStartup");
                Current.Shutdown();
            }
        }

        private void LogAndShowException(Exception ex, string source)
        {
            try
            {
                var msg = $"[{DateTime.UtcNow:O}] {source}: {ex}\n";
                var logPath = Path.Combine(Directory.GetCurrentDirectory(), "app_start_error.log");
                File.AppendAllText(logPath, msg);
                MessageBox.Show($"Startup error: {ex.Message}\nDetails written to: {logPath}", "Startup error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                // swallow any logging errors
            }
        }
    }
}
