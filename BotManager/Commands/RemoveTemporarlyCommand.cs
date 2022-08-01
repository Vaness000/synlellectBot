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
    public class RemoveTemporarlyCommand : Command
    {
        protected override string Description => 
            $"Временно отстраняет пользователя от проверки задач.{Environment.NewLine}" +
            $"{Environment.NewLine}Формат комманды:{Environment.NewLine}suspend userName." +
            $"{Environment.NewLine}Если вы хотите отстранить себя, то просто отправьте /suspend.";
        public override string CommandKey => "/suspend";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            string resultMessage = "Ошибка отстранения пользователя.";
            string userName = string.IsNullOrEmpty(commandData.UserName) ? commandData.Sender.UserName : commandData.UserName;
            LogType logType = LogType.Warning;

            if (ReviewersList.Instance.GetReviewer(userName) != null && !string.IsNullOrEmpty(commandData.AdditionalInfo))
            {
                bool isSuccess = ReviewersList.Instance.RemoveReviewer(userName, chat.Identifier.Value, false, commandData.AdditionalInfo);
                resultMessage = isSuccess ? $"Пользователь {ReviewersList.Instance.GetReviewer(userName).FullName} " +
                    $"временно удален из списка ревьюверов. {commandData.AdditionalInfo}" : resultMessage;
                logType = isSuccess ? LogType.Information : logType;
            }
            else
            {
                resultMessage += " Пользователя с таким именем не существует в списке ревьюверов или не указана причина.";
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
