using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace ODapp
{
    public partial class LoginPage : ContentPage
    {
        private readonly ISupabaseService _supabaseService;
        private readonly MainPage _mainPage;

        public LoginPage(ISupabaseService supabaseService, MainPage mainPage)
        {
            InitializeComponent();
            _supabaseService = supabaseService;
            _mainPage = mainPage;
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var email = EmailEntry.Text;
                var password = PasswordEntry.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Error", "Please enter both email and password", "OK");
                    return;
                }

                var session = await _supabaseService.Login(email, password);

                if (session != null)
                {
                    await DisplayAlert("Success", "Logged in successfully", "OK");

                    // Navigate back to the previous page or to MainPage if no history
                    var previousPage = Shell.Current.Navigation.NavigationStack.Count > 1
                        ? Shell.Current.CurrentState.Location.OriginalString
                        : "//MainPage";

                    // Navigate to ODDetailsFormPage if coming from there
                    if (previousPage.Contains("ODDetailsFormPage"))
                    {
                        await Shell.Current.GoToAsync("///ODDetailsFormPage");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync("//MainPage");
                    }

                    // Update MainPage UI
                    MainThread.BeginInvokeOnMainThread(() => {
                        _mainPage.EnableAddODDetailsButton(email);
                    });
                }
                else
                {
                    await DisplayAlert("Error", "Login failed", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                await DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
            }
        }
    }
}