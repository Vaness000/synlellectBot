using BotManager.Commands;
using BotManager.Entities;
using BotManager.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleDev
{
    class Program
    {
        static void Main(string[] args)
        {
            ReviewersList.Create(new List<Reviewer> 
            {
                //new Reviewer("Иван Рыжаев", "Vaness0000"),  
                new Reviewer("Петр Иванов", "pfff1"),
                new Reviewer("Виктор Тихонов", "pfff2"),
                new Reviewer("Илья Размахнин", "pfff3"),
                new Reviewer("Артем Чуваев", "pfff4"),
            });

            GroupList.Create();
            ControllerPrototype.Create();
            Console.Read();
        }
    }
}
