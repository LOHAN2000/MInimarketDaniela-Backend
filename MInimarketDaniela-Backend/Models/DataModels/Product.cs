using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MInimarketDaniela_Backend.Models.DataModels
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Barcode { get; set; } = string.Empty;


        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }


        public int Stock { get; set; }


        public int? CategoryId { get; set; }
        public virtual Category? Category{ get; set; }


        public int? SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }


        public virtual ICollection<SaleDetails> SaleDetails { get; set; } = new List<SaleDetails>(); // 

    }
}
