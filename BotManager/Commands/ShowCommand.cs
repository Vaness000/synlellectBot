using BotManager.Entities;
using BotManager.Logs;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotManager.Commands
{
    public class ShowCommand : Command
    {
        protected override string Description => "Отображает всех ревьюверов.";
        public override string CommandKey => "/show";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            StringBuilder resultMessages = new StringBuilder();
            var reviewers = ReviewersList.Instance.GetReviewers.Where(x => x.Chats.Contains(chat.Identifier.Value));

            if (reviewers.Count() > 0)
            {
                foreach (var user in reviewers)
                {
                    resultMessages.AppendLine(user.ToString());
                }
            }
            else
            {
                resultMessages.AppendLine("Список ревьюверов пуст");
            }

            resultMessages.AppendLine("Группы:");

            var groups = GroupList.Instance.Groups.Where(x => x.Chats.Contains(chat.Identifier.Value) || x.Name == GroupList.DefaultGroupName);

            if (groups.Count() > 0)
            {
                foreach(var group in groups)
                {
                    resultMessages.AppendLine(group.Name);
                }
            }
            else
            {
                resultMessages.AppendLine("Список групп пуст");
            }

            try
            {
                await client.SendTextMessageAsync(chat, resultMessages.ToString(), ParseMode.Markdown);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
