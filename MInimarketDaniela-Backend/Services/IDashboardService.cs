using MInimarketDaniela_Backend.DTOs;

namespace MInimarketDaniela_Backend.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}
