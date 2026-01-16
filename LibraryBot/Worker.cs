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

        public Worker(ILogger<Worker> logger, ITelegramBotClient bot)
        {
            _logger = logger;
            _bot = bot;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() };
            _bot.StartReceiving(HandleUpdate, HandleError, receiverOptions, stoppingToken);
        }

        private async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken ct)
        {
            var chatId = update.Message.Chat.Id;
            var text = update.Message.Text;
            if (text == "/start")
            {
                await bot.SendMessage(
                    chatId: chatId,
                    text: "LibraryBot has started!",
                    cancellationToken: ct);
            }
        }

        private Task HandleError(ITelegramBotClient bot, Exception ex, CancellationToken ct)
        {
            _logger.LogError(ex, "Telegram error");
            return Task.CompletedTask;
        }
    }

}
