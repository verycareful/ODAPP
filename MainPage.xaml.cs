using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using static ODapp.SupabaseService;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace ODapp
{
    public partial class MainPage : ContentPage
    {
        private readonly ISupabaseService _supabaseService;

        public ObservableCollection<Namelist> Namelist { get; set; }
        public ObservableCollection<ODDetails> ODDetails { get; set; }
        private bool _addButtonCreated = false;

        public MainPage(ISupabaseService supabaseService)
        {
            try
            {
                InitializeComponent();
                _supabaseService = supabaseService;

                Namelist = new ObservableCollection<Namelist>();
                ODDetails = new ObservableCollection<ODDetails>();

                BindingContext = this;

                Console.WriteLine("MainPage initialization completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MainPage initialization failed: {ex.Message}");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateAuthUI();
        }

        private void UpdateAuthUI()
        {
            try
            {
                bool isLoggedIn = _supabaseService.IsLoggedIn();

                if (isLoggedIn && LoginButton != null)
                {
                    LoginButton.Text = "Logout";

                    // Remove event handlers without recursively adding them
                    var logoutClickedEvent = new EventHandler(OnLogoutClicked);

                    // Clear existing handlers by cloning the button
                    var existingButton = LoginButton;
                    var newButton = new Button
                    {
                        Text = "Logout",
                        HorizontalOptions = existingButton.HorizontalOptions,
                        VerticalOptions = existingButton.VerticalOptions,
                        BackgroundColor = existingButton.BackgroundColor,
                        TextColor = existingButton.TextColor
                    };
                    newButton.Clicked += logoutClickedEvent;

                    // Replace in parent
                    var parent = (StackLayout)existingButton.Parent;
                    int index = parent.Children.IndexOf(existingButton);
                    parent.Children.RemoveAt(index);
                    parent.Children.Insert(index, newButton);
                    LoginButton = newButton;

                    // Safely add the OD Details button if it doesn't exist
                    EnableAddODDetailsButton(_supabaseService.GetCurrentUserEmail());
                }
                else if (LoginButton != null)
                {
                    LoginButton.Text = "Login";

                    // Remove event handlers without recursively adding them
                    var loginClickedEvent = new EventHandler(OnLoginClicked);

                    // Clear existing handlers by cloning the button
                    var existingButton = LoginButton;
                    var newButton = new Button
                    {
                        Text = "Login",
                        HorizontalOptions = existingButton.HorizontalOptions,
                        VerticalOptions = existingButton.VerticalOptions,
                        BackgroundColor = existingButton.BackgroundColor,
                        TextColor = existingButton.TextColor
                    };
                    newButton.Clicked += loginClickedEvent;

                    // Replace in parent
                    var parent = (StackLayout)existingButton.Parent;
                    int index = parent.Children.IndexOf(existingButton);
                    parent.Children.RemoveAt(index);
                    parent.Children.Insert(index, newButton);
                    LoginButton = newButton;

                    // Remove the Add OD Details button if it exists
                    RemoveAddODDetailsButton();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateAuthUI failed: {ex.Message}");
            }
        }

        public async Task RefreshNamelist()
        {
            try
            {
                var newNamelist = await _supabaseService.GetNamelist();

                Namelist.Clear();
                foreach (var item in newNamelist)
                {
                    Namelist.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load Namelist: {ex.Message}", "OK");
                Console.WriteLine($"RefreshNamelist failed: {ex.Message}");
            }
        }

        public async Task RefreshODDetails()
        {
            try
            {
                var newODDetails = await _supabaseService.GetODDetails();

                ODDetails.Clear();
                foreach (var item in newODDetails)
                {
                    ODDetails.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load OD Details: {ex.Message}", "OK");
                Console.WriteLine($"RefreshODDetails failed: {ex.Message}");
            }
        }

        private async void OnShowNamelistClicked(object sender, EventArgs e)
        {
            try
            {
                await RefreshNamelist();
                await Shell.Current.GoToAsync("///NamelistPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load Namelist: {ex.Message}", "OK");
                Console.WriteLine($"OnShowNamelistClicked failed: {ex.Message}");
            }
        }

        private async void OnShowODDetailsClicked(object sender, EventArgs e)
        {
            try
            {
                await RefreshODDetails();
                await Shell.Current.GoToAsync("///ODDetailsPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load OD Details: {ex.Message}", "OK");
                Console.WriteLine($"OnShowODDetailsClicked failed: {ex.Message}");
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnLoginClicked failed: {ex.Message}");
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            try
            {
                bool success = await _supabaseService.Logout();

                if (success)
                {
                    // Update UI
                    UpdateAuthUI();
                    await DisplayAlert("Success", "Logged out successfully", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Logout failed", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnLogoutClicked failed: {ex.Message}");
                await DisplayAlert("Error", $"Logout failed: {ex.Message}", "OK");
            }
        }

        private async void OnAddODDetailsClicked(object sender, EventArgs e)
        {
            try
            {
                if (_supabaseService.IsLoggedIn())
                {
                    await Shell.Current.GoToAsync("///ODDetailsFormPage");
                }
                else
                {
                    // Prompt user to login before adding OD details
                    bool result = await DisplayAlert("Authentication Required",
                        "You need to login to add OD details. Would you like to login now?", "Yes", "No");

                    if (result)
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnAddODDetailsClicked failed: {ex.Message}");
            }
        }

        private void RemoveAddODDetailsButton()
        {
            try
            {
                var addButton = ((StackLayout)Content).Children.FirstOrDefault(c => c is Button btn && btn.Text == "Add OD Details");
                if (addButton != null)
                {
                    ((StackLayout)Content).Children.Remove(addButton);
                    _addButtonCreated = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RemoveAddODDetailsButton failed: {ex.Message}");
            }
        }

        public void EnableAddODDetailsButton(string email)
        {
            try
            {
                // Prevent recursive calls or duplicate buttons
                if (_addButtonCreated)
                {
                    return;
                }

                // Update UI on main thread
                MainThread.BeginInvokeOnMainThread(() => {
                    try
                    {
                        // First check if the button already exists to avoid duplication
                        var stackLayout = Content as StackLayout;
                        var existingButton = stackLayout.Children.FirstOrDefault(c => c is Button btn && btn.Text == "Add OD Details");

                        if (existingButton == null)
                        {
                            var addODDetailsButton = new Button
                            {
                                Text = "Add OD Details"
                            };

                            addODDetailsButton.Clicked += OnAddODDetailsClicked;
                            stackLayout.Children.Insert(3, addODDetailsButton);
                            _addButtonCreated = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"EnableAddODDetailsButton UI update failed: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EnableAddODDetailsButton failed: {ex.Message}");
            }
        }
    }
    public class Namelist
    {
        [JsonProperty("REGISTER NUMBER")]
        public string RegisterNumber { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Admission Year")]
        public int AdmissionYear { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

        [JsonProperty("Course")]
        public string Course { get; set; }

        [JsonProperty("Section")]
        public string Section { get; set; }
    }

    public class ODDetails
    {
        [JsonProperty("ID")]
        [JsonIgnore]
        public long ID { get; set; }

        [JsonProperty("REGISTER NUMBER")]
        public string RegisterNumber { get; set; }

        [JsonProperty("DATE")]
        public DateTime Date { get; set; }

        [JsonProperty("FROM")]
        public short From { get; set; }

        [JsonProperty("TO")]
        public short To { get; set; }

        [JsonProperty("Reason")]
        public string Reason { get; set; }
    }
}
