using BotManager.Entities;
using BotManager.Logs;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotManager.Commands
{
    public class AddUserCommand : Command
    {
        public override string CommandKey => "/add";

        protected override string Description => 
            $"Добавляет пользователя в список ревьюверов.{Environment.NewLine}" +
            $"{Environment.NewLine}Формат комманды:{Environment.NewLine}add userName \"Имя Фамилия\"." +
            $"{Environment.NewLine}Если вы хотите добавить себя, то просто отправьте /add.";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            string resultMessage = "Ошибка добавления пользователя.";

            string fullName = string.IsNullOrEmpty(commandData.AdditionalInfo) ? commandData.Sender.FullName : commandData.AdditionalInfo;
            string userName = string.IsNullOrEmpty(commandData.UserName) ? commandData.Sender.UserName : commandData.UserName;

            LogType logType = LogType.Warning; 

            if (ReviewersList.Instance.GetReviewer(userName, chat.Identifier.Value) == null)
            {
                bool isSuccess = ReviewersList.Instance.AddReviewer(userName, fullName, chat.Identifier.Value);
                resultMessage = isSuccess ? $"Пользователь {fullName} добавлен в качестве ревьювера." : resultMessage;
                logType = isSuccess ? LogType.Information : logType;
            }
            else
            {
                resultMessage += " Пользователь с таким именем уже существует";
            }

            try
            {
                await client.SendTextMessageAsync(chat, resultMessage, ParseMode.Markdown);
                Logger.Log(logType, resultMessage);
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
