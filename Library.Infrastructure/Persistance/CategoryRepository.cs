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
    public class CategoryRepository : ICategoryRepository
    {
        public LibraryContext _libraryContext { get; set; }
        public CategoryRepository(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        

        public Task CreateCategory(Book book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategory(Book book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetCategoryById(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<PaginetedList<Book>> GetCategorys(int page, int pageSize, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategory(Book book, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
