using System;
using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FinanceManager.Data;
using FinanceManager.BLL.Services;

namespace FinanceManager.UI;

public partial class App : Application
{
    private IServiceProvider _serviceProvider;

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

            // BLL
            services.AddScoped<IFinancialProfileService, FinancialProfileService>();

            // UI
            services.AddTransient<MainWindow>();

            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
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
