using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using System.IO;

namespace ODapp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)  // Use AppContext.BaseDirectory instead of Directory.GetCurrentDirectory()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            // Register the configuration
            builder.Configuration.AddConfiguration(configuration);

            // Register services as singletons to maintain state
            builder.Services.AddSingleton<ISupabaseService, SupabaseService>();

            // Register pages
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<NamelistPage>();
            builder.Services.AddTransient<ODDetailsPage>();
            builder.Services.AddTransient<ODDetailsFormPage>();

            // Register AppShell last to avoid initialization issues
            builder.Services.AddSingleton<AppShell>();

            // Configure application lifetime events
            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                events.AddWindows(windows => windows
                    .OnActivated((window, args) => LogEvent("Windows Activated"))
                    .OnClosed((window, args) => LogEvent("Windows Closed"))
                    .OnLaunched((window, args) => LogEvent("Windows Launched"))
                    .OnVisibilityChanged((window, args) => LogEvent($"Windows Visibility Changed: {args.Visible}")));
#elif ANDROID
                events.AddAndroid(android => android
                    .OnCreate((activity, bundle) => LogEvent("Android OnCreate"))
                    .OnStart((activity) => LogEvent("Android OnStart"))
                    .OnStop((activity) => LogEvent("Android OnStop"))
                    .OnDestroy((activity) => LogEvent("Android OnDestroy"))
                    .OnPause((activity) => LogEvent("Android OnPause"))
                    .OnResume((activity) => LogEvent("Android OnResume")));
#elif IOS
                events.AddiOS(ios => ios
                    .OnActivated((app) => LogEvent("iOS OnActivated"))
                    .OnResignActivation((app) => LogEvent("iOS OnResignActivation"))
                    .DidEnterBackground((app) => LogEvent("iOS DidEnterBackground"))
                    .WillTerminate((app) => LogEvent("iOS WillTerminate")));
#endif
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void LogEvent(string eventName)
        {
            Console.WriteLine($"Lifecycle Event: {eventName}");
        }
    }
}