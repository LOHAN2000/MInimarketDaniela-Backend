namespace MInimarketDaniela_Backend.Models.DataModels
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
