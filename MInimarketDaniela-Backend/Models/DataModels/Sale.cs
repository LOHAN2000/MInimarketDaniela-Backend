using System.ComponentModel.DataAnnotations.Schema;

namespace MInimarketDaniela_Backend.Models.DataModels
{
    public class Sale : BaseEntity
    {
        public string TicketCode { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public int? UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<SaleDetails> SaleDetails { get; set; } = new List<SaleDetails>();
    }
}
