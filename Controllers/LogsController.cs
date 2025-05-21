using E_Technology_Task.Models;
using E_Technology_Task.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Technology_Task.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly ICountryBlockService _countryBlockService;

        public LogsController(ICountryBlockService countryBlockService)
        {
            _countryBlockService = countryBlockService;
        }

        [HttpGet("blocked-attempts")]
        [ProducesResponseType(typeof(IEnumerable<BlockAttemptLog>), StatusCodes.Status200OK)]
        public IActionResult GetBlockedAttempts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var logs = _countryBlockService.GetBlockAttemptLogs(page, pageSize);
            return Ok(logs);
        }
    }
}
