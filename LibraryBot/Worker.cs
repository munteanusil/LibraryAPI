using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Persistance;
using LibraryBot.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
            if (update.Message is not { Text: { } messageText })
                return;

            var chatId = update.Message.Chat.Id;
            var message = update.Message;

            using var scope = _scopeFactory.CreateAsyncScope();

            try
            {
                switch (messageText)
                {
                    case "/start":
                        await HandleStartCommand(bot, message, scope, ct);
                        break;

                    case string s when s.StartsWith("/book:"):
                        await HandleBookCommand(bot, chatId, s, scope, ct);
                        break;

                    default:
                        await bot.SendMessage(chatId, "Comandă invalidă. Folosește /start sau /book:ID", cancellationToken: ct);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la procesarea mesajului de la {ChatId}", chatId);
            }
        }

 
        private async Task HandleStartCommand(ITelegramBotClient bot, Message message, AsyncServiceScope scope, CancellationToken ct)
        {
            var chatRepository = scope.ServiceProvider.GetRequiredService<IChatRepository>();
            var chatFromDb = await chatRepository.GetChat(message.Chat.Id, ct);

            if (chatFromDb == null)
            {
                var chatToCreate = new Library.Domain.Entities.Chat
                {
                    Id = message.Chat.Id,
                    UserName = message.Chat.Username,
                    FirstName = message.Chat.FirstName,
                    LastName = message.Chat.LastName,
                    IsForm = message.Chat.IsForum,
                    Type = message.Chat.Type.ToString()
                };
                await chatRepository.CreateChat(chatToCreate, ct);
            }

            await bot.SendMessage(message.Chat.Id, "LibraryBot has started!", cancellationToken: ct);
        }

        
        private async Task HandleBookCommand(ITelegramBotClient bot, long chatId, string text, AsyncServiceScope scope, CancellationToken ct)
        {
            var parts = text.Split(':');
            if (parts.Length > 1 && int.TryParse(parts[1], out var id))
            {
                var libraryApiClient = scope.ServiceProvider.GetRequiredService<ILibraryApiClient>();

                
                var book = await libraryApiClient.GetBookById(id, ct);

                string responseText = book?.ToString() ?? "Upss Book not found!";

                await bot.SendMessage(chatId, responseText, cancellationToken: ct);
            }
            else
            {
                await bot.SendMessage(chatId, "INvalid Format! Use: /book:123", cancellationToken: ct);
            }
        }

        private Task HandleError(ITelegramBotClient bot, Exception ex, CancellationToken ct)
        {
            _logger.LogError(ex, "Telegram error");
            return Task.CompletedTask;
        }
    }

}
