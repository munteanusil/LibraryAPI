using Library.Application.Interfaces;
using Library.Domain.Common;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistance
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _libraryContext;

        public AuthorRepository(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }
        public async Task CreateAuthor(Author author, CancellationToken ct = default)
        {
            await _libraryContext.Authors.AddAsync(author,ct);
            await _libraryContext.SaveChangesAsync(ct);
        }

        public async Task DeleteAuthor(int id, CancellationToken ct = default)
        {
            var authorToRemove = await _libraryContext.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if(authorToRemove == null)
            {
                throw new KeyNotFoundException();
            }
            _libraryContext.Authors.Remove(authorToRemove);
            await _libraryContext.SaveChangesAsync();
        }

        public async Task<Author?> GetAuthorById(int id, CancellationToken ct = default)
        {
            return await _libraryContext.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id, ct);
        }

        public async Task<PaginatedList<Author>> GetAuthors(int page, int pageSize, CancellationToken ct = default)
        {
            var total = await _libraryContext.Authors.CountAsync(ct);
            var authors = await _libraryContext.Authors
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Skip((page-1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            
            return new PaginatedList<Author>(authors, page, (int)Math.Ceiling((double)total/pageSize));
        }

        public async Task UpdateAuthor(Author author, CancellationToken ct = default)
        {
             _libraryContext.Authors.Update(author);
            await _libraryContext.SaveChangesAsync(ct);
        }
    }
}
