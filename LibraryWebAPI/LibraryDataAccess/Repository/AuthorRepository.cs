using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryDataAccess.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            var authorAdded = await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return authorAdded.Entity;
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {id} was not found.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
            var existingAuthor = await _context.Authors.FindAsync(author.AuthorId);
            if (existingAuthor == null)
            {
                throw new KeyNotFoundException($"Author with ID {author.AuthorId} was not found.");
            }

           
            existingAuthor.AuthorName = author.AuthorName;

            _context.Authors.Update(existingAuthor);
            await _context.SaveChangesAsync();
            return existingAuthor;
        }
    }
}
