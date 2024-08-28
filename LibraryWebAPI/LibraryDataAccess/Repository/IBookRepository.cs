using LibraryDataAccess.Models;
using LibraryDataAccess.NewFolder;

namespace LibraryDataAccess.Repository
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book> CreateBookAsync(Book book);
        Task<Book> UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
    }
}
