using Microsoft.Maui.Controls;

namespace ODapp
{
    public partial class AppShell : Shell
    {
        private readonly ISupabaseService _supabaseService;

        public AppShell(ISupabaseService supabaseService)
        {
            InitializeComponent();
            _supabaseService = supabaseService;

            // Register routes for navigation
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("NamelistPage", typeof(NamelistPage));
            Routing.RegisterRoute("ODDetailsPage", typeof(ODDetailsPage));
            Routing.RegisterRoute("ODDetailsFormPage", typeof(ODDetailsFormPage));
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            // You can intercept navigation here if needed
            Console.WriteLine($"Navigating to: {args.Target.Location}");
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            // After navigation completes
            Console.WriteLine($"Navigated to: {args.Current?.Location}");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Check if we have a saved route to restore
                string lastRoute = Preferences.Get("last_route", null);

                if (!string.IsNullOrEmpty(lastRoute))
                {
                    Console.WriteLine($"Restoring last route: {lastRoute}");

                    // Clear the saved route to prevent loops on future launches
                    Preferences.Remove("last_route");

                    // Don't navigate if we're already on that page
                    if (Current.CurrentState?.Location?.ToString() != lastRoute)
                    {
                        await GoToAsync(lastRoute);
                    }
                }
                else
                {
                    // Default to MainPage if no saved route
                    if (Current.CurrentState?.Location?.ToString() != "//MainPage")
                    {
                        await GoToAsync("//MainPage");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AppShell.OnAppearing: {ex.Message}");

                // If restoration fails, ensure we're on a valid page
                if (Current.CurrentState == null)
                {
                    await GoToAsync("//MainPage");
                }
            }
        }
    }
}