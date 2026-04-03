using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MInimarketDaniela_Backend.Services;

namespace MInimarketDaniela_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        [Authorize(Roles = "Admin, Cajero")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var stats = await _dashboardService.GetDashboardStatsAsync();
                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error obteniendo métricas del dashboard", error = ex.Message });
            }
        }
    }
}
