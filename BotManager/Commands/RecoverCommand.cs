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
    public class RecoverCommand : Command
    {
        protected override string Description => 
            $"Восстанавливает пользователя в списке ревьюверов.{Environment.NewLine}" +
            $"{Environment.NewLine}Формат комманды:{Environment.NewLine}recover userName." +
            $"{Environment.NewLine}Если вы хотите восстановить себя, то просто отправьте /recover.";
        public override string CommandKey => "recover";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            string resultMessage = "Ошибка восстановления пользователя.";
            string userName = string.IsNullOrEmpty(commandData.UserName) ? commandData.Sender.UserName : commandData.UserName;
            LogType logType = LogType.Warning;

            if (ReviewersList.Instance.GetReviewer(userName) != null)
            {
                bool isSuccess = ReviewersList.Instance.RecoverReviewer(userName, chat.Identifier.Value);
                resultMessage = isSuccess ? $"Пользователь {ReviewersList.Instance.GetReviewer(userName).FullName} восстановлен в списке ревьюверов." : resultMessage + 
                    "Пользователь уже доступен для проверки заданий";
                logType = isSuccess ? LogType.Information : logType;

            }
            else
            {
                resultMessage += " Пользователя с таким именем не существует в списке ревьюверов.";
            }

            try
            {
                await client.SendTextMessageAsync(chat, resultMessage, ParseMode.Markdown);
                Logger.Log(logType, resultMessage);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
