using Microsoft.Maui.Controls;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Supabase;
using Supabase.Gotrue;

namespace ODapp
{
    public partial class ODDetailsFormPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private readonly Supabase.Client _supabaseClient;

        public ODDetailsFormPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            _supabaseClient = new Supabase.Client("https://aggqhrsswineotvnehuz.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImFnZ3FocnNzd2luZW90dm5laHV6Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDA4NTI1NTMsImV4cCI6MjA1NjQyODU1M30.t1-iWauDIZatoH1TZJTIigHH1Rm6sfp_wWA_f2G7eTs");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Check authentication status
            var accessToken = Preferences.Get("access_token", null);
            if (string.IsNullOrEmpty(accessToken))
            {
                // Redirect to login if not authenticated
                DisplayAlert("Authentication Required", "You need to be logged in to add OD details", "OK")
                    .ContinueWith(_ =>
                    {
                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await Shell.Current.GoToAsync("//LoginPage");
                        });
                    });
            }
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            // Validate form fields
            if (string.IsNullOrEmpty(RegisterNumberEntry.Text) ||
                string.IsNullOrEmpty(FromEntry.Text) ||
                string.IsNullOrEmpty(ToEntry.Text) ||
                string.IsNullOrEmpty(ReasonEntry.Text))
            {
                await DisplayAlert("Validation Error", "All fields are required", "OK");
                return;
            }

            // Verify that the user is authenticated
            var accessToken = Preferences.Get("access_token", null);
            if (string.IsNullOrEmpty(accessToken))
            {
                await DisplayAlert("Authentication Required", "You must be logged in to submit OD details", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            try
            {
                var newODDetail = new ODDetails
                {
                    RegisterNumber = RegisterNumberEntry.Text,
                    Date = DatePicker.Date,
                    From = short.Parse(FromEntry.Text),
                    To = short.Parse(ToEntry.Text),
                    Reason = ReasonEntry.Text
                };

                await SubmitODDetail(newODDetail, accessToken);
            }
            catch (FormatException)
            {
                await DisplayAlert("Input Error", "From and To values must be numeric", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task SubmitODDetail(ODDetails odDetail, string accessToken)
        {
            try
            {
                var url = "https://aggqhrsswineotvnehuz.supabase.co/rest/v1/ODDetails";
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("apikey", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImFnZ3FocnNzd2luZW90dm5laHV6Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDA4NTI1NTMsImV4cCI6MjA1NjQyODU1M30.t1-iWauDIZatoH1TZJTIigHH1Rm6sfp_wWA_f2G7eTs");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var jsonContent = JsonConvert.SerializeObject(odDetail);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "OD Detail submitted successfully", "OK");

                    // Clear the form
                    RegisterNumberEntry.Text = string.Empty;
                    DatePicker.Date = DateTime.Today;
                    FromEntry.Text = string.Empty;
                    ToEntry.Text = string.Empty;
                    ReasonEntry.Text = string.Empty;

                    // Navigate back to ODDetailsPage
                    await Shell.Current.GoToAsync("///ODDetailsPage");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Failed to submit OD Detail: {errorContent}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to submit OD Detail: {ex.Message}", "OK");
            }
        }
    }
}