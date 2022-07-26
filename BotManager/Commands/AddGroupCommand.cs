using BotManager.Entities;
using BotManager.Logs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotManager.Commands
{
    public class AddGroupCommand : Command
    {
        public override string CommandKey => "addgroup";

        protected override string Description => $"Добавляет новую группу.{Environment.NewLine}" +
                                                 $"Пример использования:{Environment.NewLine}addgroup groupName";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            string resultMessage = "Ошибка создания группы.";
            string groupName = commandData.UserName;
            LogType logType = LogType.Warning;

            if(!string.IsNullOrEmpty(groupName))
            {
                bool result = GroupList.Instance.AddGroup(groupName);
                resultMessage = result ? $"Группа {groupName} создана." : resultMessage += "Группа с таким именем уже существует";
                logType = result ? LogType.Information : logType;
            }

            try
            {
                await client.SendTextMessageAsync(chat, resultMessage, ParseMode.Markdown);

                Command keyboard = Command.Get(nameof(keyboard));
                await keyboard.ExecuteAsync(client, chat);

                Logger.Log(logType, resultMessage);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }

        }
    }
}
