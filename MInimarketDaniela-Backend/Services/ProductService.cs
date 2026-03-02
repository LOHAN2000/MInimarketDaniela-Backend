using Microsoft.EntityFrameworkCore;
using MInimarketDaniela_Backend.DataAccess;
using MInimarketDaniela_Backend.Models.DataModels;

namespace MInimarketDaniela_Backend.Services
{
    public class ProductService : IProductService
    {
        private readonly MinimarketContext _context;

        public ProductService(MinimarketContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .OrderBy(p => p.UpdatedAt)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id && !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync();

            if (product == null) throw new KeyNotFoundException($"Product with ID {id} not found.");

            return product;
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return await GetProductsAsync();

            term = term.ToLower();

            var products = await _context.Products
                .Where(p => !p.IsDeleted &&
                        (p.Name.ToLower().Contains(term) || p.Barcode == term))
                .Include(p => p.Category)
                .Take(20)
                .ToListAsync();

            if (products.Count == 0) throw new KeyNotFoundException($"No products found matching the term '{term}'.");

            return products;
        }

        public async Task<Product?> GetProductByBarcodeAsync(string barcode)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Barcode == barcode && !p.IsDeleted);

            if (product == null) throw new KeyNotFoundException($"Product with barcode {barcode} not found.");

            return product;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            bool existing = await _context.Products
                .AnyAsync(p => p.Barcode == product.Barcode && !p.IsDeleted);

            if (existing) throw new InvalidOperationException($"A product with barcode {product.Barcode} already exists.");

            bool categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == product.CategoryId);

            if (!categoryExists) throw new KeyNotFoundException($"Category with ID {product.CategoryId} not found.");

            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            product.CreatedBy = "Admin";

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await GetProductByIdAsync(id);

            if (existingProduct == null) throw new KeyNotFoundException($"Product with ID {id} not found.");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CostPrice = product.CostPrice;
            existingProduct.Stock = product.Stock;
            existingProduct.Barcode = product.Barcode;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.SupplierId = product.SupplierId;

            existingProduct.UpdatedAt = DateTime.UtcNow;
            existingProduct.UpdatedBy = "Admin";

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<Product?> DeleteProductAsync(int id)
        {
            var existingProduct = await GetProductByIdAsync(id);

            if (existingProduct == null) throw new KeyNotFoundException("Product with ID {id} not found.");

            existingProduct.IsDeleted = true;

            existingProduct.DeletedAt = DateTime.UtcNow;
            existingProduct.DeletedBy = "Admin";

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> CheckStockAsync(int productId, int quantity)
        {
            var product = await GetProductByIdAsync(productId);

            if (product == null) throw new KeyNotFoundException($"Product with ID {productId} not found.");

            return product.Stock >= quantity;
        }
    }
}
