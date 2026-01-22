using Supabase;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace ODapp
{
    public interface ISupabaseService
    {
        Task<Supabase.Gotrue.Session> Login(string email, string password);
        Task<bool> Logout();
        Task<ObservableCollection<Namelist>> GetNamelist();
        Task<ObservableCollection<ODDetails>> GetODDetails();
        Task<bool> SubmitODDetail(ODDetails odDetail);
        bool IsLoggedIn();
        string GetCurrentUserEmail();
    }

    public class SupabaseService : ISupabaseService
    {
        private readonly HttpClient _httpClient;
        private readonly Supabase.Client _supabaseClient;
        private bool _isAuthenticated = false;
        private string _currentUserEmail = null;

        private const string SupabaseUrl = "https://aggqhrsswineotvnehuz.supabase.co";
        private const string SupabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImFnZ3FocnNzd2luZW90dm5laHV6Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDA4NTI1NTMsImV4cCI6MjA1NjQyODU1M30.t1-iWauDIZatoH1TZJTIigHH1Rm6sfp_wWA_f2G7eTs";

        public SupabaseService()
        {
            _httpClient = new HttpClient();
            _supabaseClient = new Supabase.Client(SupabaseUrl, SupabaseKey);

            // Check if user is already authenticated
            string accessToken = Preferences.Get("access_token", null);
            if (!string.IsNullOrEmpty(accessToken))
            {
                _isAuthenticated = true;
                _currentUserEmail = Preferences.Get("user_email", null);
            }
        }

        public async Task<Supabase.Gotrue.Session> Login(string email, string password)
        {
            try
            {
                var session = await _supabaseClient.Auth.SignIn(email, password);
                
                if (session != null)
                {
                    _isAuthenticated = true;
                    _currentUserEmail = email;
                    
                    // Store session details
                    Preferences.Set("access_token", session.AccessToken);
                    Preferences.Set("refresh_token", session.RefreshToken);
                    Preferences.Set("user_email", email);
                }
                
                return session;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Logout()
        {
            try
            {
                // Clear tokens and authentication state
                Preferences.Remove("access_token");
                Preferences.Remove("refresh_token");
                Preferences.Remove("user_email");

                _isAuthenticated = false;
                _currentUserEmail = null;
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout error: {ex.Message}");
                return false;
            }
        }

        public async Task<ObservableCollection<Namelist>> GetNamelist()
        {
            try
            {
                var collection = new ObservableCollection<Namelist>();
                var url = $"{SupabaseUrl}/rest/v1/Namelist?select=*";
                
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                
                var response = await _httpClient.GetStringAsync(url);
                var namelist = JArray.Parse(response);

                foreach (var item in namelist)
                {
                    collection.Add(item.ToObject<Namelist>());
                }
                
                return collection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetNamelist error: {ex.Message}");
                return new ObservableCollection<Namelist>();
            }
        }

        public async Task<ObservableCollection<ODDetails>> GetODDetails()
        {
            try
            {
                var collection = new ObservableCollection<ODDetails>();
                var url = $"{SupabaseUrl}/rest/v1/ODDetails?select=*";
                
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                
                var response = await _httpClient.GetStringAsync(url);
                var odDetails = JArray.Parse(response);

                foreach (var item in odDetails)
                {
                    collection.Add(item.ToObject<ODDetails>());
                }
                
                return collection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetODDetails error: {ex.Message}");
                return new ObservableCollection<ODDetails>();
            }
        }

        public async Task<bool> SubmitODDetail(ODDetails odDetail)
        {
            try
            {
                var accessToken = Preferences.Get("access_token", null);
                if (string.IsNullOrEmpty(accessToken))
                {
                    return false;
                }
                
                var url = $"{SupabaseUrl}/rest/v1/ODDetails";
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("apikey", SupabaseKey);
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(odDetail);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SubmitODDetail error: {ex.Message}");
                return false;
            }
        }

        public bool IsLoggedIn()
        {
            return _isAuthenticated;
        }

        public string GetCurrentUserEmail()
        {
            return _currentUserEmail;
        }
    }
}