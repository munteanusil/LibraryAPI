using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Books;
using Library.Application.DTOs.Categories;
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

        Task<PaginatedList<AuthorDto>> GetPaginatedAuthors(int pageSize, int pageIndex, CancellationToken ct);
        Task<PaginatedList<CategoryDto>> GetPaginatedCategories(int pageSize, int pageIndex, CancellationToken ct);

        Task<AuthorDto?> GetAuthorById(int id, CancellationToken ct);

    }
}
