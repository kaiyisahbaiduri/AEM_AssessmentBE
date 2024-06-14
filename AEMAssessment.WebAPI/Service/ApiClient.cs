using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace AEMAssessment.WebAPI.Service
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var loginData = new
            {
                username = username,
                password = password
            };

            var content = new StringContent(JObject.FromObject(loginData).ToString(), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://test-demo.aemenersol.com/api/Account/Login", content);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            string responseToken = JsonConvert.DeserializeObject<string>(responseData);
            return responseToken;
        }

        public async Task<JArray> GetPlatformWellActualAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.GetAsync("http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JArray.Parse(content);
        }

        public async Task<JArray> GetPlatformWellDummyAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellDummy");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JArray.Parse(content);
        }
    }
}
