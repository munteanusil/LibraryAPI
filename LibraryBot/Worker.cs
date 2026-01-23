using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Persistance;
using LibraryBot.Interfaces;
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


            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text;
            using var scope = _scopeFactory.CreateAsyncScope();
            switch (text)
            {
                case "/start":
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
                            text: "LibraryBot has started",
                            cancellationToken: ct);
                        break;
                    }
                default:
                    {
                        var LibraryApiClient = scope.ServiceProvider.GetRequiredService<ILibraryApiClient>();
                        var book = await LibraryApiClient.GetBookById(1,ct);
                        if(book == null)
                        {
                            await bot.SendMessage(
                                chatId: chatId,
                                text: "Upss Book not found!",
                                cancellationToken: ct);

                        }
                        break;
                    }
             

        }
  
            if (text== "/start")
            {
                
            }
        }
      
        

        private Task HandleError(ITelegramBotClient bot, Exception ex, CancellationToken ct)
        {
            _logger.LogError(ex, "Telegram error");
            return Task.CompletedTask;
        }
    }

}
