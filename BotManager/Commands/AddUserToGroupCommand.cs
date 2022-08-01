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
    class AddUserToGroupCommand : Command
    {
        public override string CommandKey => "addtogroup";

        protected override string Description => $"Добавляет пользователя в указанную группу{Environment.NewLine}" +
                                                 $"Пример использования комманды:{Environment.NewLine}" +
                                                 $"addusertogroup userName \"groupName\"{Environment.NewLine}" +
                                                 $"Если вы хотите добавить себя в группу то просто напишите addusertogroup \"groupName\""; 

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            string resultMessage = "Ошибка добавления пользователя.";

            Group group = GroupList.Instance.GetGroup(commandData.AdditionalInfo, chat.Identifier.Value);
            string userName = string.IsNullOrEmpty(commandData.UserName) ? commandData.Sender.UserName : commandData.UserName;

            LogType logType = LogType.Warning;
            Reviewer reviewer = ReviewersList.Instance.GetReviewer(userName, chat.Identifier.Value);

            if(group != null && reviewer != null)
            {
                bool result = GroupList.Instance.AddReviewerToGroup(reviewer.UserName, group.Name, chat.Identifier.Value);
                resultMessage = result ? $"Пользователь {reviewer.FullName} добавлен в группу {group.Name}" : resultMessage;
                logType = result ? LogType.Information : logType;
            }
            else
            {
                resultMessage += " Указанного пользователя или группы не существует";
            }
            try
            {
                await client.SendTextMessageAsync(chat, resultMessage, ParseMode.Markdown);
                Logger.Log(logType, resultMessage);
            }
            catch(Exception e)
            {
                Logger.Log(logType, resultMessage);
            }
            
        }
    }
}
