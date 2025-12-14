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
        Task<PaginatedList<Category>> GetCategorys(int page, int pageSize, CancellationToken ct = default);

        Task<Category?> GetCategoryById(int id,CancellationToken ct = default);

        Task CreateCategory(Category category, CancellationToken ct = default);
        Task UpdateCategory(Category category, CancellationToken ct = default);
        Task DeleteCategory(int id, CancellationToken ct = default);
      
    }
}
