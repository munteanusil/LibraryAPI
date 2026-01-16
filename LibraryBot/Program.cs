using Telegram.Bot;
using Telegram.Bot.Types;

namespace LibraryBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();

            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
            {
                var token = builder.Configuration["bot_api_key"];
                return new TelegramBotClient(token);
            });   

            var host = builder.Build();
            host.Run();
        }
    }
}