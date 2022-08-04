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
    public class RemoveGroupCommand : Command
    {
        public override string CommandKey => "removegroup";

        protected override string Description => $"Удаляет группу из списка групп{Environment.NewLine}" +
                                                 $"Пример использования:{Environment.NewLine}removegroup groupName";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            string resultMessage = "Ошибка удаления группы.";
            string groupName = commandData.UserName;
            LogType logType = LogType.Warning;

            if (!string.IsNullOrEmpty(groupName))
            {
                bool result = GroupList.Instance.RemoveGroup(groupName, chat.Identifier.Value);
                resultMessage = result ? $"Группа {groupName} удалена." : resultMessage += " Группа с таким именем уже существует";
                logType = result ? LogType.Information : logType;
            }

            try
            {
                await client.SendTextMessageAsync(chat, resultMessage, ParseMode.Markdown);
                Command keyboard = Command.Get(nameof(keyboard));
                await keyboard.ExecuteAsync(client, chat, commandData);

                Logger.Log(logType, resultMessage);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
