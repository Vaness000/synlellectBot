using BotManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotManager.Commands
{
    public abstract class Command
    {
        protected static List<Command> commands = new List<Command>
        {
            new AddUserCommand(),
            new HelpCommand(),
            new RecoverCommand(),
            new RemoveTemporarlyCommand(),
            new RemoveUserCommand(),
            new ReviewCommand(),
            new ShowCommand(),
            new AddGroupCommand(),
            new AddUserToGroupCommand(),
            new RemoveGroupCommand(),
            new RemoveUserFromGroupCommand(),
            new KeyboardCommand(),
            new StartCommand()
        };


        protected abstract string Description { get; }
        public abstract string CommandKey { get; }

        public abstract Task ExecuteAsync(TelegramBotClient client, ChatId chat, CommandData commandData = null);

        public override string ToString()
        {
            return $"{CommandKey} - {Description}";
        }

        public static Command Get(string commandKey)
        {
            Command command = commands.FirstOrDefault(x => x.CommandKey.Contains(commandKey));

            if(command == null && GroupList.Instance.Groups.Contains(commandKey))
            {
                command = commands.FirstOrDefault(x => x.CommandKey == "/review");
            }

            return command;
        }

        public static Command ParseCommand(Message message, out CommandData commandData)
        {
            commandData = null;

            if (string.IsNullOrEmpty(message.Text))
            {
                return null;
            }

            commandData = GetCommandData(message.Text);
            Command command = Command.Get(commandData.CommandKey);

            if(command != null)
            {
                commandData.Sender = new CommandData.SenderInfo()
                {
                    UserName = message.From.Username,
                    FullName = $"{message.From.FirstName} {message.From.LastName}"
                };
            }

            return command;
        }

        public static CommandData GetCommandData(string message)
        {
            CommandData commandData = null;

            if (string.IsNullOrEmpty(message))
            {
                return commandData;
            }

            string[] dataFromMessage = message.Split('"', StringSplitOptions.RemoveEmptyEntries).Where(x => x != " ").ToArray();
            if(dataFromMessage.Length >= 1 && dataFromMessage.Length <= 2)
            {
                string[] importantData = dataFromMessage[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if(importantData.Length >= 1)
                {
                    commandData = new CommandData()
                    {
                        CommandKey = importantData[0]
                    };
                }
                if(importantData.Length >= 2)
                {
                    commandData.UserName = importantData[1];
                }

                if(dataFromMessage.Length >= 2)
                {
                    commandData.AdditionalInfo = dataFromMessage[1];
                }
            }

            return commandData;
        }
    }
}
