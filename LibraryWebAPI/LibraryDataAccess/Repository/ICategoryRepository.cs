using LibraryDataAccess.Models;

namespace LibraryDataAccess.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync(int page, int nr);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);

    }
}
