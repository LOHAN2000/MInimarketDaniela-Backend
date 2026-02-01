using MInimarketDaniela_Backend.Models.DataModels;

namespace MInimarketDaniela_Backend.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(string term);
        Task<Product?> GetProductByBarcodeAsync(string barcode);


        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task<Product?> DeleteProductAsync(int id);


        Task<bool> CheckStockAsync(int productId, int quantity);

    }
}
