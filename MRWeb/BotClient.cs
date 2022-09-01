using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace MRWeb
{
    public class BotClient
    {
        private readonly IConfiguration configuration;

        private static TelegramBotClient telegramBotClient;
        public static TelegramBotClient Instance
        {
            get
            {
                return telegramBotClient;
            }
        }

        private BotClient(IConfiguration configuration)
        {
            this.configuration = configuration;
            string apiKey = configuration["Token"].ToString();
            telegramBotClient = new TelegramBotClient(apiKey);
        }

        public static async Task<TelegramBotClient> Create(IConfiguration configuration)
        {
            if(telegramBotClient != null)
            {
                return telegramBotClient;
            }

            try
            {
                var bot = new BotClient(configuration);

                string webHook = configuration["WebHook"].ToString();
                string method = configuration["Method"].ToString();
                string controller = configuration["Controller"].ToString();
                string webhook = $"{webHook}{controller}{method}";

                await telegramBotClient.SetWebhookAsync(webhook);

                return telegramBotClient;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
