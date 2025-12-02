using Library.Application.Interfaces;
using Library.Domain.Common;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistance
{
    public class GenreRepository : IGenreRepository
    {
        private readonly LibraryContext _libraryContext;

        public GenreRepository(LibraryContext libraryContext)
        {
          _libraryContext = libraryContext;
        }
        public Task CreateGenre(Book book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteGenre(Book book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetGenreById(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<PaginetedList<Book>> GetGenres(int page, int pageSize, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateGenre(Book book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
