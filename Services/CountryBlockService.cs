using E_Technology_Task.Models;
using System.Collections.Concurrent;
using System.Globalization;

namespace E_Technology_Task.Services
{
    public class CountryBlockService : ICountryBlockService
    {
        private static readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries = new();
        private static readonly ConcurrentDictionary<string, DateTime> _temporalBlocks = new();
        private static readonly List<BlockAttemptLog> _blockAttemptLogs = new();
        private static readonly object _lock = new();

        public bool AddBlockedCountry(string countryCode)
        {
            if (!IsValidCountryCode(countryCode)) return false;

            return _blockedCountries.TryAdd(countryCode.ToUpper(), new BlockedCountry
            {
                Code = countryCode.ToUpper(),
                Name = GetCountryName(countryCode)
            });
        }

        public bool RemoveBlockedCountry(string countryCode)
        {
            return _blockedCountries.TryRemove(countryCode.ToUpper(), out _);
        }

        public IEnumerable<BlockedCountry> GetBlockedCountries(int page, int pageSize, string search = null)
        {
            var query = _blockedCountries.Values.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Code.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public bool AddTemporalBlock(string countryCode, int durationMinutes)
        {
            if (!IsValidCountryCode(countryCode)) return false;

            var upperCode = countryCode.ToUpper();
            var blockedUntil = DateTime.UtcNow.AddMinutes(durationMinutes);

            return _temporalBlocks.TryAdd(upperCode, blockedUntil);
        }

        public bool RemoveTemporalBlock(string countryCode)
        {
            return _temporalBlocks.TryRemove(countryCode.ToUpper(), out _);
        }

        public IEnumerable<string> GetExpiredTemporalBlocks()
        {
            var now = DateTime.UtcNow;
            return _temporalBlocks
                .Where(kv => kv.Value <= now)
                .Select(kv => kv.Key)
                .ToList();
        }

        public bool IsCountryBlocked(string countryCode)
        {
            var upperCode = countryCode.ToUpper();


            if (_blockedCountries.ContainsKey(upperCode))
                return true;


            if (_temporalBlocks.TryGetValue(upperCode, out var blockedUntil))
            {
                if (blockedUntil > DateTime.UtcNow)
                    return true;

                _temporalBlocks.TryRemove(upperCode, out _);
            }

            return false;
        }

        public void LogBlockAttempt(string ipAddress, string countryCode, bool isBlocked, string userAgent)
        {
            lock (_lock)
            {
                _blockAttemptLogs.Add(new BlockAttemptLog
                {
                    IpAddress = ipAddress,
                    CountryCode = countryCode,
                    IsBlocked = isBlocked,
                    Timestamp = DateTime.UtcNow,
                    UserAgent = userAgent
                });
            }
        }

        public IEnumerable<BlockAttemptLog> GetBlockAttemptLogs(int page, int pageSize)
        {
            lock (_lock)
            {
                return _blockAttemptLogs
                    .OrderByDescending(l => l.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
            }
        }

        private bool IsValidCountryCode(string code)
        {
            return !string.IsNullOrWhiteSpace(code) &&
                   code.Length == 2 &&
                   code.All(char.IsLetter);
        }

        private string GetCountryName(string countryCode)
        {
            try
            {
                var region = new RegionInfo(countryCode);
                return region.DisplayName;
            }
            catch
            {
                return countryCode;
            }
        }
    }


}
