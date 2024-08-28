using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.NewFolder;
using Microsoft.EntityFrameworkCore;

namespace LibraryDataAccess.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            var bookAdded = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return bookAdded.Entity; // Return the added entity
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} was not found.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            var existingBook = await _context.Books.FindAsync(book.BookId);
            if (existingBook == null)
            {
                throw new KeyNotFoundException($"Book with ID {book.BookId} was not found.");
            }

            // Update the properties of the existing book
            existingBook.Title = book.Title;
            existingBook.Price = book.Price;
            existingBook.CategoryID = book.CategoryID;
            existingBook.AuthorId = book.AuthorId;

            _context.Books.Update(existingBook);
            await _context.SaveChangesAsync();
            return existingBook;
        }
    }
}
