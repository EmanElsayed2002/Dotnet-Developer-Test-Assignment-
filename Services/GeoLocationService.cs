using E_Technology_Task.Models;
using Newtonsoft.Json;

namespace E_Technology_Task.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GeoLocationService> _logger;

        public GeoLocationService(HttpClient httpClient, IConfiguration configuration,
            ILogger<GeoLocationService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_configuration["IpApi:BaseUrl"]);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<IpInfo> GetIpInfoAsync(string ipAddress)
        {
            try
            {
                var apiKey = _configuration["IpApi:ApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                    throw new Exception("API key is missing");


                var response = await _httpClient.GetAsync($"?apiKey={apiKey}&ip={ipAddress}");
                _logger.LogError("Error with API Key: {ApiKey}", apiKey);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"API request failed with status code: {response.StatusCode}");
                    throw new Exception($"API request failed: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var ipInfo = JsonConvert.DeserializeObject<IpInfo>(content);

                if (ipInfo == null)
                    throw new Exception("Failed to deserialize IP info");

                return ipInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling geolocation API for IP {IpAddress}", ipAddress);
                throw new Exception("Geolocation service unavailable", ex);
            }
        }
    }
}
