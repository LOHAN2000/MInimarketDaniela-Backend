using System.ComponentModel.DataAnnotations.Schema;

namespace MInimarketDaniela_Backend.Models.DataModels
{
    public class SaleDetails : BaseEntity
    {
        public int? SaleId { get; set; }
        public virtual Sale? Sale { get; set; }


        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }


        public string ProductName { get; set; } = string.Empty; // Store product name at the time of sale
        public int Quantity { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }
    }
}
