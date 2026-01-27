using Library.Application.Interfaces;
using Library.Domain.Entities;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace LibraryBot
{
    [DisallowConcurrentExecution]
    public class LibraryNotificationJob : IJob
    {
        private readonly ILogger<LibraryNotificationJob> _logger;
        private readonly ITelegramBotClient _bot;
        private readonly IServiceScopeFactory _scopeFactory;

        public LibraryNotificationJob(
            ILogger<LibraryNotificationJob> logger,
            ITelegramBotClient bot,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _bot = bot;
            _scopeFactory = scopeFactory;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"{nameof(LibraryNotificationJob)} has started!");
            try
            {
                using var scope = _scopeFactory.CreateAsyncScope();
                var chatRepo = scope.ServiceProvider.GetRequiredService<IChatRepository>();
                var bookRepository = scope.ServiceProvider.GetRequiredService<IBookRepository>();
                var chats = await chatRepo.GetAllChatsForNewBookNotification();
                List<Book> books = await bookRepository.GetLatestsBooks();
                if (books.Any())
                {
                    foreach (var chat in chats)
                    {
                        await _bot.SendMessage(
                            chatId: chat.ChatId,
                            text: FormatPaginatedBooks(books),
                            parseMode: ParseMode.MarkdownV2);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation($"{nameof(LibraryNotificationJob)} has ended!");
            }
        }

        private string FormatPaginatedBooks(List<Book> books)//todo update notification text for user
        {
            var text = new StringBuilder("📚 *Books List*\n\n");
            for (int index = 0; index < books.Count(); index++)
            {
                text.Append($"Nr {index + 1} \n");
                text.Append($"📖    *{books[index].Title ?? "N/A"}*\n");
                text.Append($"ID:   *{books[index].Id}*\n");
                text.Append($"ISBN: *{books[index].ISBN}*\n");
            }
            return text.ToString();
        }

    }
}
