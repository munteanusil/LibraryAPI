using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Common;

namespace Library.Application.Interfaces
{
    public interface IGenreRepository
    {
        Task<PaginetedList<Book>> GetGenres(int page, int pageSize, CancellationToken ct = default);

        Task<Book> GetGenreById(int id,CancellationToken ct = default);

        Task CreateGenre(Book book, CancellationToken ct = default);
        Task UpdateGenre(Book book, CancellationToken ct = default);
        Task DeleteGenre(Book book, CancellationToken ct = default);
      
    }
}
