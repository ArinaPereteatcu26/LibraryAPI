using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.NewFolder;
using LibraryDataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests
{
    public class BookRepositoryTests
    {
        private List<Book> _books = new List<Book>
        {
            new Book{ BookId = 1, Title = "FirstBook", Price = 20, CategoryID = 1, AuthorId = 1 },
            new Book{ BookId = 2, Title = "SecondBook", Price = 29, CategoryID = 2, AuthorId = 2 },
            new Book{ BookId = 3, Title = "ThirdBook", Price = 40, CategoryID = 3, AuthorId = 3 },
            new Book{ BookId = 4, Title = "FourthBook", Price = 50, CategoryID = 4, AuthorId = 4 },
        };
        [Fact]
        public async Task BookRepositoryGetAllBooks_ShouldReturnZeroBooks_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);

            Assert.False(await context.Books.AnyAsync());
            var books = await repo.GetBooksAsync();
            Assert.Empty(books);
        }

        [Fact]
        public async void BookRepositoryGetAllBooks_ShouldReturnBooks_IfAny()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            await context.Books.AddRangeAsync(_books);
            await context.SaveChangesAsync();

            IBookRepository repo = new BookRepository(context);

            var books = await repo.GetBooksAsync();
            Assert.NotEmpty(books);
            Assert.Equal(_books.Count, books.Count);
            Assert.Equal(_books, books);
        }

        [Fact]
        public async void BookRepositoryGetBookById_ShouldReturnNull_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);

            var book = await repo.GetBookByIdAsync(1);
            Assert.Null(book);
        }

        [Fact]
        public async void BookRepositoryGetBookById_ShouldReturnBook_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            await context.Books.AddAsync(_books[0]);
            await context.SaveChangesAsync();

            IBookRepository repo = new BookRepository(context);

            var book = await repo.GetBookByIdAsync(_books[0].BookId);
            Assert.NotNull(book);
            Assert.Equal(_books[0].BookId, book.BookId);
        }

        [Fact]
        public async void BookRepositoryCreateBook_ShouldCreateBook_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);

            var book = await repo.CreateBookAsync(_books[0]);
            Assert.NotNull(book);
            Assert.Equal(_books[0], book);
        }

        [Fact]
        public async void BookRepositoryDeleteBook_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteBookAsync(_books[0].BookId));
        }

        [Fact]
        public async void BookRepositoryUpdateBook_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            IBookRepository repo = new BookRepository(context);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateBookAsync(_books[0]));
        }

        [Fact]
        public async void BookRepositoryUpdateBook_ShouldUpdateBook_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new LibraryContext(options);
            await context.Books.AddAsync(_books[0]);
            await context.SaveChangesAsync();

            var bookToUpdate = new Book
            {
                BookId = _books[0].BookId,
                Title = "UpdatedTitle",
                Price = 55,
                CategoryID = _books[0].CategoryID,
                AuthorId = _books[0].AuthorId
            };

            IBookRepository repo = new BookRepository(context);

            var bookUpdated = await repo.UpdateBookAsync(bookToUpdate);
            Assert.NotNull(bookUpdated);
            Assert.Equal("UpdatedTitle", bookUpdated.Title);
            Assert.Equal(55, bookUpdated.Price);
        }
    }
}
