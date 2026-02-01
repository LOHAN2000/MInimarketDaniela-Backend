using System.ComponentModel.DataAnnotations;

namespace MInimarketDaniela_Backend.Models.DataModels
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
        public DateTime? DeletedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
