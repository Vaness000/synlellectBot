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
            long chatId = chat.Identifier.Value;

            //var groups = GroupList.Instance.Groups.Where(x => x.Name == GroupList.DefaultGroupName || x.Chats.Contains(chat.Identifier.Value));

            //foreach(var group in groups)
            //{
            //    var reviewers = ReviewersList.Instance.GetReviewers.Where(x => x.Chats.Contains(chatId) && x.Groups.Contains(group.Name));
            //    resultMessages.AppendLine($"<b>{group.Name} ({reviewers.Count()})</b>");

            //    foreach(var reviewer in reviewers)
            //    {
            //        resultMessages.AppendLine($"\t{reviewer}");
            //    }
            //}

            var groups = GroupList.Instance.Groups.Where(x => x.Name == GroupList.DefaultGroupName || x.Chats.Contains(chat.Identifier.Value));

            foreach(var group in groups)
            {
                var reviewers = UserGroupsList.Instance.UserGroups.Where(x => x.ChatId == chatId && x.GroupName == group.Name)
                                                                  .Select(x => ReviewersList.Instance.GetReviewer(x.UserName));
                resultMessages.AppendLine($"<b>{group.Name} ({reviewers.Count()})</b>");

                foreach (var reviewer in reviewers)
                {
                    resultMessages.AppendLine($"\t{reviewer}");
                }
            }

            try
            {
                await client.SendTextMessageAsync(chat, resultMessages.ToString(), ParseMode.Html);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
