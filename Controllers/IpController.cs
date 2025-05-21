using E_Technology_Task.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Technology_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class IpController : ControllerBase
    {
        private readonly IGeoLocationService _geoLocationService;
        private readonly ICountryBlockService _countryBlockService;

        public IpController(
            IGeoLocationService geoLocationService,
            ICountryBlockService countryBlockService)
        {
            _geoLocationService = geoLocationService;
            _countryBlockService = countryBlockService;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> LookupIp([FromQuery] string ipAddress = null)
        {
            var ip = ipAddress ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ip) || !IsValidIp(ip))
                return BadRequest("Invalid IP address");

            try
            {
                var ipInfo = await _geoLocationService.GetIpInfoAsync(ip);
                return Ok(ipInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching IP info: {ex.Message}");
            }
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckIfIpIsBlocked()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ip))
                return BadRequest("Unable to determine IP address");

            try
            {
                var ipInfo = await _geoLocationService.GetIpInfoAsync(ip);
                var isBlocked = _countryBlockService.IsCountryBlocked(ipInfo.CountryCode);

                _countryBlockService.LogBlockAttempt(
                    ip,
                    ipInfo.CountryCode,
                    isBlocked,
                    Request.Headers.UserAgent.ToString());

                return Ok(new { IsBlocked = isBlocked, Country = ipInfo.CountryCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error checking IP block status: {ex.Message}");
            }
        }

        private bool IsValidIp(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }
    }
}
