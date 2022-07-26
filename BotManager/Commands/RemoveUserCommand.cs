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
    public class RemoveUserCommand : Command
    {
        protected override string Description => 
            $"Удаляет пользователя из списка ревьюверов.{Environment.NewLine}" +
            $"{Environment.NewLine}Формат комманды:{Environment.NewLine}remove userName." +
            $"{Environment.NewLine}Если вы хотите удалить себя, то просто отправьте /remove.";
        public override string CommandKey => "/remove";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            string resultMessage = "Ошибка удаления пользователя.";
            string userName = string.IsNullOrEmpty(commandData.UserName) ? commandData.Sender.UserName : commandData.UserName;
            LogType logType = LogType.Warning;
            Reviewer reviewer;

            if ((reviewer = ReviewersList.Instance.GetReviewer(userName)) != null)
            {
                bool isSuccess = ReviewersList.Instance.RemoveReviewer(userName, true);
                resultMessage = isSuccess ? $"Пользователь {reviewer.FullName} удален из списка ревьюверов." : resultMessage;
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
