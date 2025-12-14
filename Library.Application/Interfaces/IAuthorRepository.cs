using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Common;

namespace Library.Application.Interfaces
{
    public interface IAuthorRepository
    {
        Task<PaginatedList<Author>> GetAuthors(int page, int pageSize, CancellationToken ct = default);

        Task<Author?> GetAuthorById(int id,CancellationToken ct = default);

        Task CreateAuthor(Author author, CancellationToken ct = default);
        Task UpdateAuthor(Author author, CancellationToken ct = default);
        Task DeleteAuthor(int id, CancellationToken ct = default);
      
    }
}
