using Library.Application.DTOs.Books;
using LibraryBot.DTOS_OpenAI;
using LibraryBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibraryBot.Implementations
{
    public class OpenAiService : IOpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public OpenAiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.OpenAiClient);
            _model = configuration["OpenAi:Model"];
        }

        public async Task<string> GetBookRecommandation(BookDto book, int count, CancellationToken ct)
        {
            var prompt = $"Based on the following book:\n" +
               $"- Title: {book.Title}\n" +
               $"- Author: {book.Author.FirstName}\n" +
               $"- Category: {book.Category.Name}\n\n" +
               $"Please give me {count} similar book recommendations.";
            return await SendOpenAiRequest(prompt, ct);
        }

        private async Task<string?> SendOpenAiRequest(string prompt, CancellationToken ct)
        {
            var request = new OpenAiRequest
            {
                Model = _model,
                Messages = new[]
                {
                    new OpenAiMessage
                    {
                        Role = "system",
                        Content = "You are a librarian assistant that helps users with suggestions about similar books of what they provide. Your responses allways are in markdown format with special characters escaped."
                    },
                    new OpenAiMessage
                    {
                        Role = "user",
                        Content = prompt
                    }
                },
                Temperature = 0.7
            };

            var json = JsonSerializer.Serialize(request, _options);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("v1/chat/completions", content, ct);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                throw new Exception($"OpenAi api error: {error} with status: {response.StatusCode}");
            }

            var responseJson = await response.Content.ReadAsStringAsync(ct);
            var openAiResponse = JsonSerializer.Deserialize<OpenAiResponse>(responseJson, _options);
            return openAiResponse?.Choices.FirstOrDefault()?.Message.Content;
        }
    }
}
