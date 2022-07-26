using BotManager.Entities;
using BotManager.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotManager.Commands
{
    public class HelpCommand : Command
    {
        protected override string Description => "Отображает справку";
        public override string CommandKey => "/help";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            StringBuilder commandsDescription = new StringBuilder();

            foreach(Command command in commands.Where(x => x.CommandKey != "/start"))
            {
                commandsDescription.AppendLine(command.ToString());
                commandsDescription.AppendLine(Environment.NewLine);
            }

            try
            {
                await client.SendTextMessageAsync(chat, commandsDescription.ToString(), ParseMode.Markdown);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
