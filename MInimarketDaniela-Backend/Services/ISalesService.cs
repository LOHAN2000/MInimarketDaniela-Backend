using MInimarketDaniela_Backend.DTOs;
using MInimarketDaniela_Backend.Models.DataModels;

namespace MInimarketDaniela_Backend.Services
{
    public interface ISalesService
    {
        Task<Sale> CreateSaleAsync(CreateSaleDto saleDto);
        Task<Sale?> GetSaleByTicketAsync(string ticketCode);
    }
}
