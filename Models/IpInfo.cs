using Newtonsoft.Json;

namespace E_Technology_Task.Models
{
    public class IpInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("country_code2")]
        public string CountryCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("isp")]
        public string Isp { get; set; }
    }
}
