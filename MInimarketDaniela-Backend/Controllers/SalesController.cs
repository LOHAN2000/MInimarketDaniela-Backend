using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MInimarketDaniela_Backend.DTOs;
using MInimarketDaniela_Backend.Services;

namespace MInimarketDaniela_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Cajero")]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleDto saleDto)
        {
            try
            {
                var sale = await _salesService.CreateSaleAsync(saleDto);
                return Ok(new
                {
                    message = "Venta exitosa",
                    ticketCode = sale.TicketCode,
                    total = sale.Total,
                    sale
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("ticket/{code}")]
        [Authorize(Roles = "Admin, Cajero")]
        public async Task<IActionResult> GetByTicket(string code)
        {
            try
            {
                var ticket = await _salesService.GetSaleByTicketAsync(code);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
