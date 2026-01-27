using Library.Application.DTOs.Authors;
using Library.Application.DTOs.Books;
using Library.Application.DTOs.Categories;
using Library.Application.Interfaces;
using Library.Domain.Common;
using Library.Domain.Entities;
using Library.Infrastructure.Persistance;
using LibraryBot.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LibraryBot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, ITelegramBotClient bot,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _bot = bot;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiver = new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() };
            _bot.StartReceiving(HandleUpdate, HandleError, receiver, stoppingToken);
        }

        private async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken ct)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallback(bot, update.CallbackQuery, ct);
                return;
            }

            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text;

            using var scope = _scopeFactory.CreateAsyncScope();
            switch (text)
            {
                case string s when s.Equals("/start"):
                    {
                        var chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                        var chatFromDb = await chatRepository.GetChat(chatId, ct);
                        if (chatFromDb == null)
                        {
                            var chatToCreate = new Library.Domain.Entities.Chat
                            {
                                Id = update.Message.Chat.Id,
                                UserName = update.Message.Chat.Username,
                                FirstName = update.Message.Chat.FirstName,
                                LastName = update.Message.Chat.LastName,
                                IsForm = update.Message.Chat.IsForum,
                                Type = update.Message.Chat.Type.ToString()
                            };
                            await chatRepository.CreateChat(chatToCreate, ct);
                        }
                        await bot.SendMessage(
                            chatId: chatId,
                            text: GetWelcomeMessage(),
                            parseMode: ParseMode.MarkdownV2,
                            replyMarkup: GetMainMenuKeyboard(),
                            cancellationToken: ct);
                        break;
                    }
                case string s when s.StartsWith("/book:"):
                    {
                        var libraryApiClient = scope.ServiceProvider.GetRequiredService<ILibraryApiClient>();
                        var splits = s.Split(":", StringSplitOptions.RemoveEmptyEntries);
                        if (splits.Count() == 2 &&
                            int.TryParse(splits[1], out var id))
                        {
                            var book = await libraryApiClient.GetBookById(id, ct);
                            if (book == null)
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: "Upss book not found!",
                                   cancellationToken: ct);
                            }
                            else
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: book.ToString(),
                                   cancellationToken: ct);
                            }
                        }
                        else
                        {
                            await bot.SendMessage(
                                   chatId: chatId,
                                   text: "Invalid command",
                                   cancellationToken: ct);
                        }

                        break;
                    }
                case string s when s.StartsWith("/books:"):
                    {
                        var libraryApiClient = scope.ServiceProvider.GetRequiredService<ILibraryApiClient>();
                        var splits = s.Split(":", StringSplitOptions.RemoveEmptyEntries);
                        if (splits.Count() == 3 &&
                            int.TryParse(splits[1], out var pageSize)
                            && int.TryParse(splits[2], out var pageIndex))
                        {
                            var paginatedBooks = await libraryApiClient.GetPaginatedBooks(pageSize, pageIndex, ct);
                            if (paginatedBooks == null)
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: AvailableCommands(),
                                   cancellationToken: ct);
                            }
                            else
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: paginatedBooks.ToString(),
                                   cancellationToken: ct);
                            }
                        }
                        else
                        {
                            await bot.SendMessage(
                                   chatId: chatId,
                                   text: "Invalid command",
                                   cancellationToken: ct);
                        }

                        break;
                    }
                case string s when s.StartsWith("/author:"):
                    {
                        var libraryApiClient = scope.ServiceProvider.GetRequiredService<ILibraryApiClient>();
                        var splits = s.Split(":", StringSplitOptions.RemoveEmptyEntries);
                        if (splits.Count() == 2 &&
                            int.TryParse(splits[1], out var id))
                        {
                            var author = await libraryApiClient.GetAuthorById(id, ct);
                            if (author == null)
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: "Upss author not found!",
                                   cancellationToken: ct);
                            }
                            else
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: author.ToString(),
                                   cancellationToken: ct);
                            }
                        }
                        else
                        {
                            await bot.SendMessage(
                                   chatId: chatId,
                                   text: "Invalid command",
                                   cancellationToken: ct);
                        }

                        break;
                    }
                case string s when s.StartsWith("/authors:"):
                    {
                        var libraryApiClient = scope.ServiceProvider.GetRequiredService<ILibraryApiClient>();
                        var splits = s.Split(":", StringSplitOptions.RemoveEmptyEntries);
                        if (splits.Count() == 3 &&
                            int.TryParse(splits[1], out var pageSize)
                            && int.TryParse(splits[2], out var pageIndex))
                        {
                            var paginatedAuthors = await libraryApiClient.GetPaginatedAuthors(pageSize, pageIndex, ct);
                            if (paginatedAuthors == null)
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: AvailableCommands(),
                                   cancellationToken: ct);
                            }
                            else
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: paginatedAuthors.ToString(),
                                   cancellationToken: ct);
                            }
                        }
                        else
                        {
                            await bot.SendMessage(
                                   chatId: chatId,
                                   text: "Invalid command",
                                   cancellationToken: ct);
                        }

                        break;
                    }
                case string s when s.StartsWith("/categories:"):
                    {
                        var libraryApiClient = scope.ServiceProvider.GetRequiredService<ILibraryApiClient>();
                        var splits = s.Split(":", StringSplitOptions.RemoveEmptyEntries);
                        if (splits.Count() == 3 &&
                            int.TryParse(splits[1], out var pageSize)
                            && int.TryParse(splits[2], out var pageIndex))
                        {
                            var paginatedCategories = await libraryApiClient.GetPaginatedCategories(pageSize, pageIndex, ct);
                            if (paginatedCategories == null)
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: AvailableCommands(),
                                   cancellationToken: ct);
                            }
                            else
                            {
                                await bot.SendMessage(
                                   chatId: chatId,
                                   text: paginatedCategories.ToString(),
                                   cancellationToken: ct);
                            }
                        }
                        else
                        {
                            await bot.SendMessage(
                                   chatId: chatId,
                                   text: "Invalid command",
                                   cancellationToken: ct);
                        }

                        break;
                    }
                case string s when s.StartsWith("/recomendation:"):
                    {
                        var libraryApiClient = scope.ServiceProvider.GetRequiredService<ILibraryApiClient>();
                        var splits = s.Split(":", StringSplitOptions.RemoveEmptyEntries);

                        if (splits.Length == 2 && int.TryParse(splits[1], out var id))
                        {
                            var book = await libraryApiClient.GetBookById(id, ct);
                            if (book == null)
                            {
                                await bot.SendMessage(chatId, "❌ Book not found!", cancellationToken: ct);
                            }
                            else
                            {
                                try
                                {
                                   
                                    var loadingMsg = await bot.SendMessage(chatId, "🤖 Generate recomandations...", cancellationToken: ct);

                                    var openAiService = scope.ServiceProvider.GetRequiredService<IOpenAiService>();
                                    var result = await openAiService.GetBookRecommandation(book, 3, ct);

                                    await bot.SendMessage(chatId, $"✨ recomandations for '{book.Title}':\n\n{result}", cancellationToken: ct);
                                }
                                catch (Exception ex)
                                {
                                    await bot.SendMessage(chatId, $"⚠️ Error for services Ai: {ex.Message}", cancellationToken: ct);
                                }
                            }
                        }
                        break;
                    }
                default:
                    await bot.SendMessage(
                               chatId: chatId,
                               text: "Invalid command",
                               cancellationToken: ct);
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    break;
            }
        }

        private async Task HandleCallback(ITelegramBotClient bot, CallbackQuery callbackQuery, CancellationToken ct)
        {
            var data = callbackQuery.Data;
            var chatId = callbackQuery.Message.Chat.Id;

            if (data == "main_menu")
            {
                await bot.EditMessageText(
                    chatId: chatId,
                    messageId: callbackQuery.Message.MessageId,
                    text: GetWelcomeMessage(),
                    parseMode: ParseMode.MarkdownV2,
                    replyMarkup: GetMainMenuKeyboard());
                return;
            }
            if (data.StartsWith("books_") || data.StartsWith("authors_") || data.StartsWith("categories_"))
            {
                using var scope = _scopeFactory.CreateAsyncScope();
                var parts = data.Split("_");
                var type = parts[0];
                var pageSize = int.Parse(parts[1]);
                var pageIndex = int.Parse(parts[2]);
                var libraryApiClient = scope.ServiceProvider.GetRequiredService<ILibraryApiClient>();
                switch (type)
                {
                    case "books":
                        var books = await libraryApiClient.GetPaginatedBooks(pageSize, pageIndex, ct);
                        await bot.EditMessageText(
                            chatId: chatId,
                            messageId: callbackQuery.Message.MessageId,
                            text: FormatPaginatedBooks(books),
                            parseMode: ParseMode.MarkdownV2,
                            replyMarkup: GetPaginatedKeyboard("books", pageSize, pageIndex, books.TotalPages)
                            );
                        break;
                    case "authors":
                        var authors = await libraryApiClient.GetPaginatedAuthors(pageSize, pageIndex, ct);
                        await bot.EditMessageText(
                            chatId: chatId,
                            messageId: callbackQuery.Message.MessageId,
                            text: FormatPaginatedAuthors(authors),
                            parseMode: ParseMode.MarkdownV2,
                            replyMarkup: GetPaginatedKeyboard("authors", pageSize, pageIndex, authors.TotalPages)
                            );
                        break;
                    case "categories":
                        var categories = await libraryApiClient.GetPaginatedCategories(pageSize, pageIndex, ct);
                        await bot.EditMessageText(
                            chatId: chatId,
                            messageId: callbackQuery.Message.MessageId,
                            text: FormatPaginatedCategories(categories),
                            parseMode: ParseMode.MarkdownV2,
                            replyMarkup: GetPaginatedKeyboard("categories", pageSize, pageIndex, categories.TotalPages)
                            );
                        break;
                }
            }
            await bot.AnswerCallbackQuery(callbackQuery.Id, cancellationToken: ct);
        }

        private Task HandleError(ITelegramBotClient bot, Exception ex, CancellationToken ct)
        {
            _logger.LogError(ex, "Telegram error");
            return Task.CompletedTask;
        }

        private string GetWelcomeMessage()
        {
            return @"📚 *Welcome to Library Bot\!*
                    
            I can help you browse our library catalog\.
            Choose an option:

            📖 *Books* \- Browse all books
            ✍️ *Authors* \- Browse all authors
            📂 *Categories* \- Browse all categories

            You can also use direct commands:
             `/book:id` \- Get book by ID
             `/author:id` \- Get author by ";
        }

        private InlineKeyboardMarkup GetMainMenuKeyboard()
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("📖 Browse Books", "books_10_1"),
                    InlineKeyboardButton.WithCallbackData("✍️ Browse Authors", "authors_10_1")
                },
                new[]{
                    InlineKeyboardButton.WithCallbackData("📂 Browse Categories", "categories_10_1")
                }
            });
        }

        private string FormatPaginatedBooks(PaginatedList<BookDto> paginatedBooks)
        {
            var text = new StringBuilder("📚 *Books List*\n\n");
            foreach (var book in paginatedBooks.Items)
            {
                text.Append($"📖    *{book.Title ?? "N/A"}*\n");
                text.Append($"ID:   *{book.Id}*\n");
                text.Append($"ISBN: *{book.ISBN}*\n");
            }
            text.Append($"\n Page {paginatedBooks.PageNumber} of {paginatedBooks.TotalPages}");
            return text.ToString();
        }

        private string FormatPaginatedAuthors(PaginatedList<AuthorDto> paginatedAuthors)
        {
            var text = new StringBuilder("✍️ *Authors List*\n\n");
            foreach (var author in paginatedAuthors.Items)
            {
                text.Append($"🧑‍🏫    *{author.FirstName ?? "N/A"}*\n");
                text.Append($"ID:   *{author.Id}*\n");
                text.Append($"Books: *{author.Books?.Count() ?? 0}*\n");//to update api to include author books
            }
            text.Append($"\n Page {paginatedAuthors.PageNumber} of {paginatedAuthors.TotalPages}");
            return text.ToString();
        }
        private string FormatPaginatedCategories(PaginatedList<CategoryDto> paginatedCategories)
        {
            var text = new StringBuilder("📂 *Categories List*\n\n");
            foreach (var category in paginatedCategories.Items)
            {
                text.Append($"📂    *{category.Name ?? "N/A"}*\n");
                text.Append($"ID:   *{category.Id}*\n");
                text.Append($"Books: *{category.Books}*\n");//to update api to include author books
            }
            text.Append($"\n Page {paginatedCategories.PageNumber} of {paginatedCategories.TotalPages}");
            return text.ToString();
        }

        private InlineKeyboardMarkup GetPaginatedKeyboard(string type, int pageSize, int currentPage, int totalPages)
        {
            var buttons = new List<InlineKeyboardButton[]>();
            var navigationButtons = new List<InlineKeyboardButton>();
            if (currentPage > 1)
            {
                navigationButtons.Add(InlineKeyboardButton.WithCallbackData("⬅️ Privious", $"{type}_{pageSize}_{currentPage - 1}"));
            }
            navigationButtons.Add(InlineKeyboardButton.WithCallbackData($"📄 {currentPage}/{totalPages}", "invalid-command"));
            if (currentPage < totalPages)
            {

                navigationButtons.Add(InlineKeyboardButton.WithCallbackData("Next ➡️", $"{type}_{pageSize}_{currentPage + 1}"));
            }
            buttons.Add(navigationButtons.ToArray());
            buttons.Add(new[] { InlineKeyboardButton.WithCallbackData("🔙 Back to Menu", "main_menu") });
            return new InlineKeyboardMarkup(buttons);
        }

        private string AvailableCommands()
        {
            return "/book:<id> - returns book by id \n" +
                "/books:<pageSize>:<pageIndex> returns paginated books \n" +
                "/author:<id> - returns author by id \n" +
                "/authors:<pageSize>:<pageIndex> returns paginated authors \n" +
                "/catories:<pageSize>:<pageIndex> returns paginated categories";
        }
    }

}
