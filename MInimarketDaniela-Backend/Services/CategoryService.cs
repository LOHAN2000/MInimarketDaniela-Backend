using Microsoft.EntityFrameworkCore;
using MInimarketDaniela_Backend.DataAccess;
using MInimarketDaniela_Backend.Models.DataModels;

namespace MInimarketDaniela_Backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly MinimarketContext _context;

        public CategoryService(MinimarketContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Products.Where(p => !p.IsDeleted))
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id && !c.IsDeleted)
                .Include(c => c.Products.Where(p => !p.IsDeleted))
                .FirstOrDefaultAsync();

            if (category == null) throw new KeyNotFoundException($"Category with ID {id} not found.");

            return category;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            var nameExists = await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower() && !c.IsDeleted);

            if (nameExists) throw new InvalidOperationException($"Category with name '{category.Name}' already exists.");

            category.CreatedAt = DateTime.UtcNow;
            category.CreatedBy = "Admin";
            category.IsDeleted = false;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (existingCategory == null || existingCategory.IsDeleted) throw new KeyNotFoundException($"Category with ID {id} not found.");

            var nameExists = await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower() && c.Id != id && !c.IsDeleted);

            if (nameExists) throw new InvalidOperationException($"Category with name '{category.Name}' already exists.");

            existingCategory.Name = category.Name;

            existingCategory.UpdatedAt = DateTime.UtcNow;
            existingCategory.UpdatedBy = "Admin";

            await _context.SaveChangesAsync();

            return existingCategory;
        }

        public async Task<Category> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null || category.IsDeleted) throw new KeyNotFoundException($"Category with ID {id} not found.");

            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id && !p.IsDeleted);

            if (hasProducts) throw new InvalidOperationException("Cannot delete category with active products.");

            category.IsDeleted = true;
            category.DeletedBy = "Admin";
            category.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return category;
        }
    }
}
