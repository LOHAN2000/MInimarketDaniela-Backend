using System.ComponentModel.DataAnnotations;

namespace MInimarketDaniela_Backend.DTOs
{
    public class CreateSupplierDto
    {
        [Required]
        public string RUC { get; set; } = string.Empty;

        [Required]
        public string BussinessName { get; set; } = string.Empty;

        public string TradeName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }

    public class UpdateSupplierDto : CreateSupplierDto
    {
        [Required]
        public int Id { get; set; }
    }
}
