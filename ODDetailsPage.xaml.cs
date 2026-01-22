using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace ODapp
{
    public partial class ODDetailsPage : ContentPage
    {
        private readonly MainPage _mainPage;

        public ODDetailsPage(MainPage mainPage)
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

        private async void OnRefreshODDetailsClicked(object sender, EventArgs e)
        {
            try
            {
                if (_mainPage != null)
                {
                    await _mainPage.RefreshODDetails();
                }
                else
                {
                    Console.WriteLine("MainPage reference is null");
                    await DisplayAlert("Error", "Cannot refresh - application state error", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnRefreshODDetailsClicked failed: {ex.Message}");
                await DisplayAlert("Error", $"Failed to refresh OD Details: {ex.Message}", "OK");
            }
        }
    }
}