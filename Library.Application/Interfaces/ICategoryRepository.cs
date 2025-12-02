using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Common;

namespace Library.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PaginetedList<Book>> GetCategorys(int page, int pageSize, CancellationToken ct = default);

        Task<Book> GetCategoryById(int id,CancellationToken ct = default);

        Task CreateCategory(Book book, CancellationToken ct = default);
        Task UpdateCategory(Book book, CancellationToken ct = default);
        Task DeleteCategory(Book book, CancellationToken ct = default);
      
    }
}
