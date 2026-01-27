using Library.Infrastructure.Extensions;
using LibraryBot.Implementations;
using LibraryBot.Interfaces;
using Quartz;
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
            builder.Services.AddScoped<ILibraryApiClient, LibraryApiClient>();
            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
            {
                var token = builder.Configuration["bot_api_key"];
                return new TelegramBotClient(token);
            });

            builder.Services.AddHttpClient(Constants.LibraryApiClient, (_, c) =>
            {
                c.BaseAddress = new Uri(builder.Configuration["LibraryApiConfig:BaseAddress"]);
                c.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                c.DefaultRequestHeaders.Add("x-app-name", builder.Configuration["LibraryApiConfig:AppName"]);
            });

            builder.Services.AddHttpClient(Constants.OpenAiClient, (_, c) =>
            {
                c.BaseAddress = new Uri(builder.Configuration["OpenAi:BaseAddress"]);
                c.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                c.DefaultRequestHeaders.Add("Authorization", "Bearer " + builder.Configuration["OpenAi:ApiKey"]);
            });
            builder.Services.AddScoped<IOpenAiService, OpenAiService>();

            builder.Services.AddQuartz(q =>
            {
                var notificatJobKey = new JobKey(nameof(LibraryNotificationJob));
                q.AddJob<LibraryNotificationJob>(opts => opts.WithIdentity(notificatJobKey));
                q.AddTrigger(opts =>
                opts.ForJob(notificatJobKey)
                .WithIdentity($"{notificatJobKey}_trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(30)
                    .RepeatForever()
                    .WithMisfireHandlingInstructionNextWithExistingCount()));
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            var host = builder.Build();
            host.Run();
        }

    }
}