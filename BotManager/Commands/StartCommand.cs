using BotManager.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotManager.Commands
{
    public class StartCommand : Command
    {
        public override string CommandKey => "/start";

        protected override string Description => string.Empty;

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            Command command = commands.First(x => x.CommandKey == "/keyboard");
            try
            {
                await command.ExecuteAsync(client, chat);
                Logger.Log(LogType.Information, $"Бот запущен в чате {chat.Identifier}");
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
