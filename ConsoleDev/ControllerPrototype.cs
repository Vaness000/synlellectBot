using BotManager.Commands;
using BotManager.Entities;
using BotManager.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleDev
{
    public class ControllerPrototype
    {
        private const string token = "5518674534:AAEcwGsQXe3VQabM9yt-witfsOuNnpNRvA0";

        private TelegramBotClient client;
        private static ReceiverOptions receiverOptions;
        private ReplyKeyboardMarkup keyboard;

        private ControllerPrototype()
        {
            receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage
                }
            };

            keyboard = new ReplyKeyboardMarkup(new KeyboardButton[]
            {
                new KeyboardButton("add"), new KeyboardButton("suspend")
            });


            client = new TelegramBotClient(token);
            client.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);
        }

        public static ControllerPrototype Create()
        {
            return new ControllerPrototype();
        }
        private Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            await Update(update);
        }

        public async Task Update(Update update)
        {
            if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
            {
                Command command = Command.ParseCommand(update.Message, out CommandData commandData);
                if (command != null)
                {
                    try
                    {
                        await command.ExecuteAsync(client, update.Message.Chat, commandData);
                    }
                    catch(Exception e)
                    {
                        Logger.Log(LogType.Error, e.Message);
                    }
                }
            }
        }

        //just for tests
        public static IEnumerable<IEnumerable<string>> GetButtons()
        {
            List<string> allButtons = GroupList.Instance.Groups.Concat(Buttons.Get).ToList();
            List<List<string>> buttonsMarkup = new List<List<string>>();
            int counter = 0;

            foreach (string button in allButtons)
            {
                if (counter % 2 == 0)
                {
                    buttonsMarkup.Add(new List<string>());
                }

                buttonsMarkup[counter / 2].Add(button);
                counter++;
            }

            return buttonsMarkup;
        }
    }
}
