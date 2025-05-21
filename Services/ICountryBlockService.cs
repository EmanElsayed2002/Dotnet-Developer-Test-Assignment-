using E_Technology_Task.Models;

namespace E_Technology_Task.Services
{
    public interface ICountryBlockService
    {
        bool AddBlockedCountry(string countryCode);
        bool RemoveBlockedCountry(string countryCode);
        IEnumerable<BlockedCountry> GetBlockedCountries(int page, int pageSize, string search = null);
        bool AddTemporalBlock(string countryCode, int durationMinutes);
        bool RemoveTemporalBlock(string countryCode);
        IEnumerable<string> GetExpiredTemporalBlocks();
        bool IsCountryBlocked(string countryCode);
        void LogBlockAttempt(string ipAddress, string countryCode, bool isBlocked, string userAgent);
        IEnumerable<BlockAttemptLog> GetBlockAttemptLogs(int page, int pageSize);
    }
}
