﻿using BotManager.Entities;
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
    class RemoveUserFromGroupCommand : Command
    {
        public override string CommandKey => "removefromgroup";

        protected override string Description => $"Удаляет пользователя из указанной группы{Environment.NewLine}" +
                                                  $"Пример использования комманды:{Environment.NewLine}" +
                                                  $"removefromgroup userName \"groupName\"{Environment.NewLine}" +
                                                  $"Если вы хотите удалить себя из группы то просто напишите removefromgroup \"groupName\"";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            string resultMessage = "Ошибка добавления пользователя.";

            Group group = GroupList.Instance.GetGroup(commandData.AdditionalInfo, chat.Identifier.Value);
            string userName = string.IsNullOrEmpty(commandData.UserName) ? commandData.Sender.UserName : commandData.UserName;
            Reviewer reviewer = ReviewersList.Instance.GetReviewer(userName);
            LogType logType = LogType.Warning;

            if (group != null && reviewer != null)
            {
                bool result = UserGroupsList.Instance.RemoveUserFromGroup(chat.Identifier.Value, reviewer.UserName, group.Name);
                resultMessage = result ? $"Пользователь {reviewer.FullName} удален из группы {group.Name}" : resultMessage;
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
                Logger.Log(LogType.Error, e.Message);
            }
        }
    }
}
