using E_Technology_Task.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Technology_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CountriesController : ControllerBase
    {
        private readonly ICountryBlockService _countryBlockService;

        public CountriesController(ICountryBlockService countryBlockService)
        {
            _countryBlockService = countryBlockService;
        }

        [HttpPost("block")]
        public IActionResult BlockCountry([FromBody] string countryCode)
        {
            if (string.IsNullOrEmpty(countryCode) || countryCode.Length != 2)
                return BadRequest("Invalid country code");

            if (_countryBlockService.AddBlockedCountry(countryCode))
                return Ok();

            return Conflict("Country is already blocked");
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode)
        {
            if (_countryBlockService.RemoveBlockedCountry(countryCode))
                return Ok();

            return NotFound("Country is not blocked");
        }

        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = null)
        {
            var countries = _countryBlockService.GetBlockedCountries(page, pageSize, search);
            return Ok(countries);
        }

        [HttpPost("temporal-block")]
        public IActionResult TemporarilyBlockCountry(
            [FromBody] TemporalBlockRequest request)
        {
            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes");

            if (string.IsNullOrEmpty(request.CountryCode) || request.CountryCode.Length != 2)
                return BadRequest("Invalid country code");

            if (_countryBlockService.AddTemporalBlock(request.CountryCode, request.DurationMinutes))
                return Ok();

            return Conflict("Country is already temporarily blocked");
        }
    }

    public class TemporalBlockRequest
    {
        public string CountryCode { get; set; }
        public int DurationMinutes { get; set; }
    }
}
