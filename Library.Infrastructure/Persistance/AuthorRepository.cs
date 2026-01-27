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
        private static readonly Func<LibraryContext, int, Task<Author?>> GetAuthorByIdCompiled = EF.CompileAsyncQuery((LibraryContext context, int id) => context.Authors.FirstOrDefault(a => a.Id == id));

        public AuthorRepository(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }
        public async Task CreateAuthor(Author book, CancellationToken ct = default)
        {
            await _libraryContext.Authors.AddAsync(book, ct);
            await _libraryContext.SaveChangesAsync(ct);
        }

        public async Task DeleteAuthor(int id, CancellationToken ct = default)
        {
            var authorToRemove = await _libraryContext.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (authorToRemove == null)
            {
                throw new KeyNotFoundException();
            }
            _libraryContext.Authors.Remove(authorToRemove);
            await _libraryContext.SaveChangesAsync();
        }

        public async Task<Author?> GetAuthorById(int id, CancellationToken ct = default) =>
            await GetAuthorByIdCompiled(_libraryContext, id);

        public async Task<PaginatedList<Author>> GetAuthors(int page, int pageSize, CancellationToken ct = default)
        {
            var total = await _libraryContext.Authors.CountAsync(ct);
            var authors = await _libraryContext.Authors
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Include(a => a.Books)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PaginatedList<Author>(authors, page, (int)Math.Ceiling((double)total / pageSize));
        }

        public async Task UpdateAuthor(Author author, CancellationToken ct = default)
        {
            if (author.AuthorGenres != null)
            {
                var existingAuthorGenres = await _libraryContext.AuthorGenres.AsNoTracking().Where(a => a.AuthorId == author.Id).ToListAsync(ct);
                var genresToAdd = new List<AuthorGenres>();
                var genresToRemove = new List<AuthorGenres>();
                foreach (var genre in author.AuthorGenres)
                {
                    if (!existingAuthorGenres.Any(g => g.GenreId == genre.GenreId))
                    {
                        genresToAdd.Add(genre);
                    }
                }

                foreach (var genre in existingAuthorGenres)
                {
                    if (!author.AuthorGenres.Any(g => g.GenreId == genre.GenreId))
                    {
                        genresToRemove.Add(genre);
                    }
                }

                _libraryContext.AuthorGenres.RemoveRange(genresToRemove);
                await _libraryContext.AuthorGenres.AddRangeAsync(genresToAdd);
            }

            _libraryContext.Authors.Update(author);
            await _libraryContext.SaveChangesAsync(ct);
        }
    }
}