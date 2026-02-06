using Microsoft.EntityFrameworkCore;
using MInimarketDaniela_Backend.DataAccess;
using MInimarketDaniela_Backend.DTOs;
using MInimarketDaniela_Backend.Models.DataModels;

namespace MInimarketDaniela_Backend.Services
{
    public class SalesService : ISalesService
    {
        private readonly MinimarketContext _context;

        public SalesService(MinimarketContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateSaleAsync(CreateSaleDto saleDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newSale = new Sale
                {
                    TicketCode = $"T-{DateTime.UtcNow.Ticks.ToString().Substring(10)}",
                    PaymentMethod = saleDto.PaymentMethod,
                    UserId = saleDto.UserId,

                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "system",
                    IsDeleted = false
                };

                decimal grandTotal = 0;

                foreach (var item in saleDto.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);

                    if (product == null) throw new Exception($"Producto con ID {item.ProductId} no encontrado.");
                    if (product.IsDeleted) throw new Exception($"Producto con ID {item.ProductId} ha sido eliminado.");

                    if (product.Stock < item.Quantity) throw new Exception($"Stock insuficiente para el producto {product.Name}.");

                    product.Stock -= item.Quantity;

                    var detail = new SaleDetails
                    {
                        Product = product,
                        Sale = newSale,
                        ProductName = product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        SubTotal = product.Price * item.Quantity,
                        CreatedBy = "Cajero",
                    };

                    grandTotal += detail.SubTotal;
                    newSale.SaleDetails.Add(detail);
                }

                newSale.Total = grandTotal;

                _context.Sales.Add(newSale);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return newSale;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear la venta: {ex.Message}");

            }
        }

        public async Task<Sale?> GetSaleByTicketAsync(string ticketCode)
        {
            var ticket = await _context.Sales
                .Where(s => !s.IsDeleted)
                .Include(t => t.SaleDetails)
                .FirstOrDefaultAsync(s => s.TicketCode == ticketCode);

            if (ticket == null) throw new Exception($"Venta con código de ticket {ticketCode} no encontrada.");

            return ticket;
        }
    }
}
