using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Common;

namespace Library.Application.Interfaces
{
    public interface IBookRepository
    {
        Task<PaginetedList<Book>> GetBooks(int page, int pageSize, CancellationToken ct = default);

        Task<Book> GetBookById(int id,CancellationToken ct = default);

        Task CreateBook(Book book, CancellationToken ct = default);
        Task UpdateBook(Book book, CancellationToken ct = default);
        Task DeleteBook(int id, CancellationToken ct = default);
      
    }
}
