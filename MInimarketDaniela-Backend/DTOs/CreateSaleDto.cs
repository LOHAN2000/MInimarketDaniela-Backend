namespace MInimarketDaniela_Backend.DTOs
{
    public class CreateSaleDto
    {
        public string PaymentMethod { get; set; } = "Efectivo";
        public int? UserId { get; set; }

        public List<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();
    }
}
