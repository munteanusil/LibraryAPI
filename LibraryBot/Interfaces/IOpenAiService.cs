using Library.Application.DTOs.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBot.Interfaces
{
    public interface IOpenAiService
    {
        Task<string> GetBookRecommandation(BookDto book, int count = 3, CancellationToken ct = default);
    
    }
}
