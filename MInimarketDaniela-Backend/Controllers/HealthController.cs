using Microsoft.AspNetCore.Mvc;

namespace MInimarketDaniela_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new
            {
                status = "Live",
                time = DateTime.UtcNow.ToString("g")
            });
        }
    }
}
