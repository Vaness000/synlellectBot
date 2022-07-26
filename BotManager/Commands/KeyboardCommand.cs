﻿using BotManager.Entities;
using BotManager.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotManager.Commands
{
    class KeyboardCommand : Command
    {
        public override string CommandKey => "/keyboard";

        protected override string Description => "Отображает клавиатуру для взаимодействия";

        public override async Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null)
        {
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(GetButtons());

            try
            {
                await client.SendTextMessageAsync(chat, "Выберете действие", replyMarkup: keyboard);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }

        private static IEnumerable<IEnumerable<KeyboardButton>> GetButtons()
        {
            List<string> allButtons = GroupList.Instance.Groups.Concat(Buttons.Get).ToList();
            List<List<KeyboardButton>> buttonsMarkup = new List<List<KeyboardButton>>();
            int counter = 0;

            foreach(string button in allButtons)
            {
                if (counter % 2 == 0)
                {
                    buttonsMarkup.Add(new List<KeyboardButton>());
                }

                buttonsMarkup[counter / 2].Add(new KeyboardButton(button));
                counter++;
            }

            return buttonsMarkup;
        }
    }
}
