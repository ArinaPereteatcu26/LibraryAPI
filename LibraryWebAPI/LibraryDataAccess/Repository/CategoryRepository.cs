

using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryDataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository

    {
        private readonly LibraryContext _context;
        public CategoryRepository(LibraryContext context) 
        {
            _context = context;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
           var categoryAdded = await  _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} was not found.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with ID {category.CategoryId} was not found.");
            }
            existingCategory.Name = category.Name;

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;
        }

    }
}
