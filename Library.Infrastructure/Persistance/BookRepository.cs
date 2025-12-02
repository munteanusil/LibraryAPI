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
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _libraryContext;

        public BookRepository(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }
        public Task CreateBook(Book book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBook(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetBookById(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<PaginetedList<Book>> GetBooks(int page, int pageSize, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateBook(Book book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
