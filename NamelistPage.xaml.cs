using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace ODapp
{
    public partial class NamelistPage : ContentPage
    {
        private readonly MainPage _mainPage;

        public NamelistPage(MainPage mainPage)
        {
            InitializeComponent();
            _mainPage = mainPage;
            BindingContext = _mainPage;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // No need to search for MainPage as we now have it injected
        }

        private async void OnRefreshNamelistClicked(object sender, EventArgs e)
        {
            try
            {
                if (_mainPage != null)
                {
                    await _mainPage.RefreshNamelist();
                }
                else
                {
                    Console.WriteLine("MainPage reference is null");
                    await DisplayAlert("Error", "Cannot refresh - application state error", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnRefreshNamelistClicked failed: {ex.Message}");
                await DisplayAlert("Error", $"Failed to refresh Namelist: {ex.Message}", "OK");
            }
        }
    }
}