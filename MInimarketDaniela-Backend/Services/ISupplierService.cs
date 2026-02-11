using MInimarketDaniela_Backend.DTOs;
using MInimarketDaniela_Backend.Models.DataModels;

namespace MInimarketDaniela_Backend.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier> GetByIdAsync(int id);
        Task<Supplier> CreateAsync(CreateSupplierDto dto);
        Task<Supplier> UpdateAsync(int id, UpdateSupplierDto dto);
        Task<Supplier?> DeleteAsync(int id);
    }
}
