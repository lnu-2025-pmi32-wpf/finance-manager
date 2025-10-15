using System;
using System.IO;
using FinanceManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Photino.Blazor;



namespace FinanceManager
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);
            
            // Configuration setup
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            appBuilder.Services
                .AddSingleton<IConfiguration>(configuration)
                .AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
                .AddFluentUIComponents()
                .AddLogging();

            // register root component and selector
            appBuilder.RootComponents.Add<App>("app");

            var app = appBuilder.Build();

#if DEBUG
            // Seed the database with test data during development
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DataSeeder.Seed(dbContext);
            }
#endif

            // customize window
            app.MainWindow
                .SetIconFile("favicon.ico")
                .SetTitle("Finance Manager");

            AppDomain.CurrentDomain.UnhandledException += (sender, error) => { app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString()); };

            app.Run();
        }
    }
}
