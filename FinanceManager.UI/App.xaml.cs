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
        private static readonly string LogDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
        private static readonly string LogFilePath = Path.Combine(LogDirectory, "app.log");

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
                services.AddScoped<IAnalyticsService, FinanceManager.BLL.Services.AnalyticsService>();

                // UI
                // App logging
                services.AddSingleton<Services.IAppLogger, Services.FileLogger>();

                services.AddTransient<MainWindow>();
                services.AddTransient<Views.DashboardView>();
                services.AddScoped<ViewModels.DashboardViewModel>();
                services.AddTransient<Views.CategoriesView>();
                services.AddScoped<ViewModels.CategoriesViewModel>();
                services.AddTransient<Views.AccountsView>();
                services.AddScoped<ViewModels.AccountsViewModel>();
                services.AddTransient<Views.TransactionsView>();
                services.AddScoped<ViewModels.TransactionsViewModel>();
                services.AddTransient<Views.AnalyticsView>();
                services.AddScoped<ViewModels.AnalyticsViewModel>();

                this.serviceProvider = services.BuildServiceProvider();

                // Seed DB in development (uses DAL DataSeeder)
                using (var scope = this.serviceProvider.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    FinanceManager.Data.DataSeeder.Seed(db);
                }

                var mainWindow = this.serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();

                // Log application start (use DI logger if available)
                try
                {
                    var logger = this.serviceProvider.GetService<Services.IAppLogger>();
                    if (logger != null)
                    {
                        logger.Info("Application started");
                    }
                    else
                    {
                        WriteLog("INFO", "Application started");
                    }
                }
                catch
                {
                    // swallow logging errors during startup
                }
            }
            catch (Exception ex)
            {
                LogAndShowException(ex, "OnStartup");
                Current.Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                var logger = this.serviceProvider?.GetService<Services.IAppLogger>();
                if (logger != null)
                {
                    logger.Info("Application exiting");
                }
                else
                {
                    WriteLog("INFO", "Application exiting");
                }
            }
            catch
            {
                // swallow logging errors
            }

            base.OnExit(e);
        }

        private void LogAndShowException(Exception ex, string source)
        {
            try
            {
                var fullMsg = $"[{DateTime.UtcNow:O}] {source}: {ex}\n";

                // Prefer DI logger when available
                try
                {
                    var logger = this.serviceProvider?.GetService<Services.IAppLogger>();
                    if (logger != null)
                    {
                        logger.Error(fullMsg);
                    }
                    else
                    {
                        // Fallback to file-based logging
                        WriteLog("ERROR", $"{source}: {ex}");
                    }
                }
                catch
                {
                    // If anything goes wrong with service-based logging, fallback
                    WriteLog("ERROR", $"{source}: {ex}");
                }

                // Also show the exception to the user as before
                var displayLogPath = LogFilePath;
                MessageBox.Show($"Startup error: {ex.Message}\nDetails written to: {displayLogPath}", "Startup error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                // swallow any logging errors
            }
        }

        private static void WriteLog(string level, string message)
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }

                var line = $"[{DateTime.UtcNow:O}] {level}: {message}{Environment.NewLine}";
                File.AppendAllText(LogFilePath, line);
            }
            catch
            {
                // swallow - logging should not crash the app
            }
        }
    }
}
