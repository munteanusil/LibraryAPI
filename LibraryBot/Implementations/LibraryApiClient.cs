using Library.Application.DTOs.Books;
using Library.Domain.Common;
using LibraryBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBot.Implementations
{
    public class LibraryApiClient : ILibraryApiClient
    {
        private readonly HttpClient _client;
        public LibraryApiClient(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(Constants.LibraryApiClient);
        }
        public async Task<BookDto> GetBookById(int id, CancellationToken ct)
        {
            return await _client.GetFromJsonAsync<BookDto?>($"/Books/{id}", ct);
        }

        public Task<PaginatedList<BookDto>> GetPaginatedBooks(int pageSize, int pageIndex, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
