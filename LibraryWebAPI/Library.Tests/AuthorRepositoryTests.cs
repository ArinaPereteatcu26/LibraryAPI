using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.NewFolder;
using LibraryDataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Library.Tests
{
    public class AuthorRepositoryTests
    {
        private List<Author> _authors = new List<Author>
        {
            new Author { AuthorId = 1, AuthorName = "Author One" },
            new Author { AuthorId = 2, AuthorName = "Author Two" },
            new Author { AuthorId = 3, AuthorName = "Author Three" },
            new Author { AuthorId = 4, AuthorName = "Author Four" },
        };

        [Fact]
        public async Task AuthorRepositoryGetAllAuthors_ShouldReturnZeroAuthors_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.False(await context.Authors.AnyAsync());
            var authors = await repo.GetAuthorsAsync();
            Assert.Empty(authors);
        }

        [Fact]
        public async void AuthorRepositoryGetAllAuthors_ShouldReturnAuthors_IfAny()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            await context.Authors.AddRangeAsync(_authors);
            await context.SaveChangesAsync();
            IAuthorRepository repo = new AuthorRepository(context);

            var authors = await repo.GetAuthorsAsync();
            Assert.NotEmpty(authors);
            Assert.Equal(_authors.Count, authors.Count);
        }
        [Fact]
        public async Task AuthorRepositoryGetAuthorById_ShouldReturnNull_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);

            var author = await repo.GetAuthorByIdAsync(1);
            Assert.Null(author);
        }


        [Fact]
        public async Task AuthorRepositoryGetAuthorById_ShouldReturnAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            await context.Authors.AddAsync(_authors[0]);
            await context.SaveChangesAsync();

            IAuthorRepository repo = new AuthorRepository(context);

            var author = await repo.GetAuthorByIdAsync(_authors[0].AuthorId);
            Assert.NotNull(author);
            Assert.Equal(_authors[0].AuthorId, author.AuthorId);
        }

        [Fact]
        public async Task AuthorRepositoryCreateAuthor_ShouldCreateAuthor_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);

            var author = await repo.CreateAuthorAsync(_authors[0]);
            Assert.NotNull(author);
            Assert.Equal(_authors[0].AuthorName, author.AuthorName);
        }

        [Fact]
        public async Task AuthorRepositoryDeleteAuthor_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAuthorAsync(_authors[0].AuthorId));
        }

        [Fact]
        public async Task AuthorRepositoryUpdateAuthor_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAuthorAsync(_authors[0]));
        }


        [Fact]
        public async Task AuthorRepositoryUpdateAuthor_ShouldUpdateAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            await context.Authors.AddAsync(_authors[0]);
            await context.SaveChangesAsync();

            var authorToUpdate = new Author
            {
                AuthorId = _authors[0].AuthorId,
                AuthorName = "UpdatedAuthorName"
            };

            IAuthorRepository repo = new AuthorRepository(context);

            var authorUpdated = await repo.UpdateAuthorAsync(authorToUpdate);
            Assert.NotNull(authorUpdated);
            Assert.Equal("UpdatedAuthorName", authorUpdated.AuthorName);
        }
    }
}