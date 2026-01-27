using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Books;
using Library.Application.DTOs.Categories;
using Library.Domain.Common;
using LibraryBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibraryBot.Implementations
{
    public class LibraryApiClient : ILibraryApiClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<LibraryApiClient> _logger;

        public LibraryApiClient(IHttpClientFactory httpClientFactory, ILogger<LibraryApiClient> logger)
        {
            _client = httpClientFactory.CreateClient(Constants.LibraryApiClient);
            _logger = logger;
        }

        public async Task<AuthorDto?> GetAuthorById(int id, CancellationToken ct)
        {
            try
            {
                return await _client.GetFromJsonAsync<AuthorDto?>($"/authors/{id}", ct);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }

        public async Task<BookDto?> GetBookById(int id, CancellationToken ct)
        {
            try
            {
                return await _client.GetFromJsonAsync<BookDto?>($"/books/{id}", ct);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }

        public async Task<PaginatedList<AuthorDto>> GetPaginatedAuthors(int pageSize, int pageIndex, CancellationToken ct)
        {
            try
            {
                return await _client.GetFromJsonAsync<PaginatedList<AuthorDto>>($"/authors?pageSize={pageSize}&page={pageIndex}", ct);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new PaginatedList<AuthorDto>(new List<AuthorDto>(), 0, 0);
            }
        }

        public async Task<PaginatedList<BookDto>> GetPaginatedBooks(int pageSize, int pageIndex, CancellationToken ct)
        {
            try
            {
                return await _client.GetFromJsonAsync<PaginatedList<BookDto>>($"/books?pageSize={pageSize}&page={pageIndex}", ct);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new PaginatedList<BookDto>(new List<BookDto>(), 0, 0);
            }
        }

        public async Task<PaginatedList<CategoryDto>> GetPaginatedCategories(int pageSize, int pageIndex, CancellationToken ct)
        {
            try
            {
                return await _client.GetFromJsonAsync<PaginatedList<CategoryDto>>($"/category?pageSize={pageSize}&page={pageIndex}", ct);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new PaginatedList<CategoryDto>(new List<CategoryDto>(), 0, 0);
            }
        }
    }

}

