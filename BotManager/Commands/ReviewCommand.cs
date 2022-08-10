using BotManager.Entities;
using BotManager.Logs;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotManager.Commands
{
    public class ReviewCommand : Command
    {
        protected override string Description => "Выбирает ревьювера для проверки задачи.";
        public override string CommandKey => "/review";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {

            Reviewer reviewer = ReviewersList.Instance.GetReviewerToCheckTask(commandData.Sender.UserName, chat.Identifier.Value, commandData.CommandKey.Replace("/", string.Empty));
            string resultMessage = reviewer != null ? reviewer.FullName : "Проверять некому, список пуст или нет доступных ревьюверов";
            try
            {
                await client.SendTextMessageAsync(chat, resultMessage, ParseMode.Markdown);
                Logger.Log(LogType.Information, resultMessage);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
