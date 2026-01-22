using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace ODapp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            MainPage = serviceProvider.GetRequiredService<AppShell>();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);

            // Attach to window events
            if (window != null)
            {
                window.Created += OnWindowCreated;
                window.Destroying += OnWindowDestroying;

                Console.WriteLine("Window created");
            }

            return window;
        }

        protected override void OnStart()
        {
            base.OnStart();
            Console.WriteLine("Application started");
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            Console.WriteLine("Application going to sleep");
            SaveApplicationState();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Console.WriteLine("Application resuming");
        }

        private void OnWindowCreated(object sender, EventArgs e)
        {
            Console.WriteLine("Window created event received");
        }

        private void OnWindowDestroying(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Window closing");
                SaveApplicationState();
                CleanupResources();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnWindowDestroying: {ex.Message}");
            }
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Process exiting");
                SaveApplicationState();
                CleanupResources();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnProcessExit: {ex.Message}");
            }
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Console.WriteLine($"Unhandled exception: {e.ExceptionObject}");
                SaveApplicationState();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnUnhandledException: {ex.Message}");
            }
        }

        private void SaveApplicationState()
        {
            try
            {
                Console.WriteLine("Saving application state");

                var currentRoute = Shell.Current?.CurrentState?.Location?.ToString();
                if (!string.IsNullOrEmpty(currentRoute))
                {
                    Preferences.Set("last_route", currentRoute);
                    Console.WriteLine($"Saved last route: {currentRoute}");
                }

                var supabaseService = _serviceProvider?.GetService<ISupabaseService>();
                if (supabaseService != null)
                {
                    Console.WriteLine("Supabase service accessed for state saving");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving application state: {ex.Message}");
            }
        }

        private void CleanupResources()
        {
            try
            {
                Console.WriteLine("Cleaning up resources");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cleaning up resources: {ex.Message}");
            }
        }
    }
}
