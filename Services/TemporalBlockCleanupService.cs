namespace E_Technology_Task.Services
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly ICountryBlockService _countryBlockService;
        private readonly ILogger<TemporalBlockCleanupService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(5);

        public TemporalBlockCleanupService(
            ICountryBlockService countryBlockService,
            ILogger<TemporalBlockCleanupService> logger)
        {
            _countryBlockService = countryBlockService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);

            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    var expiredBlocks = _countryBlockService.GetExpiredTemporalBlocks().ToList();

                    foreach (var countryCode in expiredBlocks)
                    {
                        _countryBlockService.RemoveTemporalBlock(countryCode);
                        _logger.LogInformation("Removed expired temporal block for {CountryCode}", countryCode);
                    }

                    if (expiredBlocks.Any())
                    {
                        _logger.LogInformation("Cleaned up {Count} expired temporal blocks", expiredBlocks.Count);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cleaning up temporal blocks");
                }
            }
        }
    }
}
