using System.ComponentModel.DataAnnotations;

namespace MInimarketDaniela_Backend.Models.DataModels
{
    public class Supplier : BaseEntity
    {
        [Required]
        public string RUC { get; set; } = string.Empty;

        [Required]
        public string BussinessName { get; set; } = string.Empty;

        public string TradeName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;


        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
