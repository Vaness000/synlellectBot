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
            StringBuilder users = new StringBuilder();
            if(ReviewersList.Instance.Reviewers.Count > 0)
            {
                foreach (var user in ReviewersList.Instance.Reviewers)
                {
                    users.AppendLine(user.ToString());
                }
            }
            else
            {
                users.AppendLine("Список ревьюверов пуст");
            }

            users.AppendLine("Группы:");

            if(GroupList.Instance.Groups.Count > 0)
            {
                foreach(var group in GroupList.Instance.Groups)
                {
                    users.AppendLine(group);
                }
            }
            else
            {
                users.AppendLine("Список групп пуст");
            }

            try
            {
                await client.SendTextMessageAsync(chat, users.ToString(), ParseMode.Markdown);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
