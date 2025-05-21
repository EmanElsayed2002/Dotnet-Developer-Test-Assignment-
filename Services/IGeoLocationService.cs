using E_Technology_Task.Models;

namespace E_Technology_Task.Services
{
    public interface IGeoLocationService
    {
        Task<IpInfo> GetIpInfoAsync(string ipAddress);
    }
}
