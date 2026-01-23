using Library.Application.DTOs.Books;
using Library.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBot.Interfaces
{
    public interface ILibraryApiClient
    {
        Task<PaginatedList<BookDto>> GetPaginatedBooks(int pageSize, int pageIndex, CancellationToken ct);

        Task<BookDto?> GetBookById(int id, CancellationToken ct);
        
    }
}
