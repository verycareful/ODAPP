using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ODapp
{
    public static class SupabaseAuth
    {
        private static string currentUserEmail;

        public static string GetCurrentUserEmail()
        {
            return currentUserEmail;
        }

        public static async Task<bool> LoginUsingHttpClient(string email, string password)
        {
            try
            {
                var httpClient = new HttpClient();
                var jsonContent = JsonConvert.SerializeObject(new { email = email, password = password });
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Add the API key to the headers
                httpClient.DefaultRequestHeaders.Add("apikey", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImFnZ3FocnNzd2luZW90dm5laHV6Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDA4NTI1NTMsImV4cCI6MjA1NjQyODU1M30.t1-iWauDIZatoH1TZJTIigHH1Rm6sfp_wWA_f2G7eTs");

                var response = await httpClient.PostAsync("https://aggqhrsswineotvnehuz.supabase.co/auth/v1/token?grant_type=password", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Login successful: " + responseBody);
                    currentUserEmail = email;
                    return true;
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Login failed: " + errorBody);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return false;
            }
        }
    }
}
