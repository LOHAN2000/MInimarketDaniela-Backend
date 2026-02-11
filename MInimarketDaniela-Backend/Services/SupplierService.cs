using Microsoft.EntityFrameworkCore;
using MInimarketDaniela_Backend.DataAccess;
using MInimarketDaniela_Backend.DTOs;
using MInimarketDaniela_Backend.Models.DataModels;
using System.Security.Claims; // Necesario para ClaimTypes
using Microsoft.AspNetCore.Http; //

namespace MInimarketDaniela_Backend.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly MinimarketContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupplierService(MinimarketContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers
                .Where(s => !s.IsDeleted)
                .Include(s => s.Products.Where(p => !p.IsDeleted))
                .OrderBy(s => s.Products.Count(p => !p.IsDeleted))
                .ToListAsync();
        }

        public async Task<Supplier> GetByIdAsync(int id)
        {
            var supplier = await _context.Suppliers
                .Where(s => s.Id == id && !s.IsDeleted)
                .Include(s => s.Products.Where(p => !p.IsDeleted))
                .FirstOrDefaultAsync();

            if (supplier == null) throw new KeyNotFoundException($"Supplier with ID {id} not found.");

            return supplier;
        }

        public async Task<Supplier> CreateAsync(CreateSupplierDto dto)
        {
            if (await _context.Suppliers.AnyAsync(s => s.RUC == dto.RUC && !s.IsDeleted))
                throw new InvalidOperationException($"Supplier with RUC {dto.RUC} already exists.");

            var cajeroActual = ObtenerUsuarioActual();

            var supplier = new Supplier
            {
                RUC = dto.RUC,
                BussinessName = dto.BussinessName,
                TradeName = dto.TradeName,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = cajeroActual,
                IsDeleted = false
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return supplier;
        }

        public async Task<Supplier> UpdateAsync(int id, UpdateSupplierDto dto)
        {
            var supplier = await _context.Suppliers.Where(s => s.Id == id && !s.IsDeleted).FirstOrDefaultAsync();

            if (supplier == null) throw new KeyNotFoundException($"Supplier with ID {id} not found.");

            if (await _context.Suppliers.AnyAsync(s => s.RUC == dto.RUC && s.Id != id && !s.IsDeleted))
                throw new InvalidOperationException($"Another supplier with RUC {dto.RUC} already exists.");

            var usuarioActual = ObtenerUsuarioActual();

            supplier.RUC = dto.RUC;
            supplier.BussinessName = dto.BussinessName;
            supplier.TradeName = dto.TradeName;
            supplier.Phone = dto.Phone;
            supplier.Email = dto.Email;
            supplier.Address = dto.Address;

            supplier.UpdatedAt = DateTime.UtcNow;
            supplier.UpdatedBy = usuarioActual;

            await _context.SaveChangesAsync();
            return supplier;
        }

        public async Task<Supplier?> DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.Where(s => s.Id == id && !s.IsDeleted).FirstOrDefaultAsync();

            if (supplier == null) throw new KeyNotFoundException($"Supplier with ID {id} not found.");

            var hasActiveProducts = await _context.Products.AnyAsync(p => p.SupplierId == id && !p.IsDeleted);

            if (hasActiveProducts) throw new InvalidOperationException($"Cannot delete supplier with ID {id} because it has active products.");

            var usuarioActual = ObtenerUsuarioActual();

            supplier.IsDeleted = true;
            supplier.DeletedAt = DateTime.UtcNow;
            supplier.DeletedBy = usuarioActual;

            await _context.SaveChangesAsync();

            return supplier;
        }


        private string ObtenerUsuarioActual()
        {
            var username = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            return username ?? "System";
        }
    }
}
