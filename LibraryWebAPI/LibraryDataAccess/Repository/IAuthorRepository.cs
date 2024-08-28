using LibraryDataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryDataAccess.Repository
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAuthorsAsync(int page, int nr);
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<Author> CreateAuthorAsync(Author author);
        Task<Author> UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(int id);
    }
}
