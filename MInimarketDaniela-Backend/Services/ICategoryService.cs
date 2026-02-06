using MInimarketDaniela_Backend.Models.DataModels;

namespace MInimarketDaniela_Backend.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(int id, Category category);
        Task<Category> DeleteCategoryAsync(int id);
    }
}
