using Library.Infrastructure.Extensions;
using LibraryBot.Implementations;
using LibraryBot.Interfaces;
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
            builder.Services.ConfigureRepositories();
            builder.Services.ConfigureEFCore(builder.Configuration);
            builder.Services.AddScoped<ILibraryApiClient,LibraryApiClient>();
            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
            {
                var token = builder.Configuration["bot_api_key"];
                return new TelegramBotClient(token);
            });
           

            builder.Services.AddHttpClient(Constants.LibraryApiClient,(_,c) =>
            {
                c.BaseAddress = new Uri(builder.Configuration["LibraryApiConfig:BaseAdress"]);
                c.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                c.DefaultRequestHeaders.Add("x-app-name", builder.Configuration["LibraryApiConfig: AppName"]);
            });
            var host = builder.Build();
            host.Run();
        }
    }
}