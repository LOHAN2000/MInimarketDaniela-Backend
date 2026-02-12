using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MInimarketDaniela_Backend.DTOs;
using MInimarketDaniela_Backend.Services;

namespace MInimarketDaniela_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ISunatService _sunatService;

        public SuppliersController(ISupplierService suppplierService, ISunatService sunatService)
        {
            _supplierService = suppplierService;
            _sunatService = sunatService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Cajero")]
        public async Task<IActionResult> GetAll()
        {
            var providers = await _supplierService.GetAllAsync();
            return Ok(providers);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Cajero")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var provider = await _supplierService.GetByIdAsync(id);
                return Ok(provider);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSupplierDto dto)
        {
            try
            {
                var createdProvider = await _supplierService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new
                {
                    id = createdProvider.Id,
                    createdProvider
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto dto)
        {
            try
            {
                var updatedProvider = await _supplierService.UpdateAsync(id, dto);
                return Ok(updatedProvider);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedProvider = await _supplierService.DeleteAsync(id);
                return Ok(deletedProvider);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("sunat/{ruc}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSunatInfo(string ruc)
        {
            try
            {
                var info = await _sunatService.ConsultaRucAsync(ruc);
                return Ok(info);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
