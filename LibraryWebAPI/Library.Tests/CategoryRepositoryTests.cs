using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests
{
    public class CategoryRepositoryTests
    {
        private List<Category> _categories = new List<Category>
        {
            new Category{ CategoryId = 1, Name = "FirstCategory"},
            new Category{ CategoryId = 2, Name = "SecondCategory"},
            new Category{ CategoryId = 3, Name = "ThirdCategory"},
            new Category{ CategoryId = 4, Name = "FourthCategory"},
        };

        [Fact]
        public async void CategoryRepositoryGetAllCategories_ShouldReturnZeroCategories_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);

            Assert.False(context.Categories.Any());
            var categories = await repo.GetCategoriesAsync();
            Assert.Empty(categories);
        }

        [Fact]
        public async void CategoryRepositoryGetAllCategories_ShouldReturnCategories_IfAny()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            await context.Categories.AddRangeAsync(_categories);
            await context.SaveChangesAsync();

            ICategoryRepository repo = new CategoryRepository(context);

            var categories = await repo.GetCategoriesAsync();
            Assert.NotEmpty(categories);
            Assert.Equal(_categories.Count, categories.Count);
            Assert.Equal(_categories, categories);
        }

        [Fact]
        public async void CategoryRepositoryGetCategoryById_ShouldReturnNull_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);

            var category = await repo.GetCategoryByIdAsync(1);
            Assert.Null(category);
        }

        [Fact]
        public async void CategoryRepositoryGetCategoryById_ShouldReturnCategory_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            await context.Categories.AddAsync(_categories[0]);
            await context.SaveChangesAsync();

            ICategoryRepository repo = new CategoryRepository(context);

            var category = await repo.GetCategoryByIdAsync(_categories[0].CategoryId);
            Assert.NotNull(category);
            Assert.Equal(_categories[0].CategoryId, category.CategoryId);
        }

        [Fact]
        public async void CategoryRepositoryCreateCategory_ShouldCreateCategory_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);

            var category = await repo.CreateCategoryAsync(_categories[0]);
            Assert.NotNull(category);
            Assert.Equal(_categories[0], category);
        }

        [Fact]
        public async void CategoryRepositoryDeleteCategory_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteCategoryAsync(_categories[0].CategoryId));
        }

        [Fact]
        public async void CategoryRepositoryUpdateCategory_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateCategoryAsync(_categories[0]));
        }

        [Fact]
        public async void CategoryRepositoryUpdateCategory_ShouldUpdateCategory_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            await context.Categories.AddAsync(_categories[0]);
            await context.SaveChangesAsync();

            var categoryToUpdate = new Category
            {
                Name = "UpdatedName",
                CategoryId = _categories[0].CategoryId
            };

            ICategoryRepository repo = new CategoryRepository(context);

            var categoryUpdated = await repo.UpdateCategoryAsync(categoryToUpdate);
            Assert.NotNull(categoryUpdated);
            Assert.Equal("UpdatedName", categoryUpdated.Name);
        }
    }
}
