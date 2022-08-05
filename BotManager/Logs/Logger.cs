using BotManager.Entities;
using System;
using System.IO;

namespace BotManager.Logs
{
    public class Logger
    {
        private static string path = Path.Combine(Environment.CurrentDirectory, "log.txt");

        public static void Log(LogType type, string logMessage)
        {
            File.AppendAllText(path, $"{DateTime.Now} {type}: {logMessage}{Environment.NewLine}");
            if(type == LogType.Information)
            {
                Serializer<GroupList>.Serialize(GroupList.Instance);
                Serializer<ReviewersList>.Serialize(ReviewersList.Instance);
                DataProvider.Instance.Insert(ReviewersList.Instance);
                DataProvider.Instance.Insert(GroupList.Instance);
                DataProvider.Instance.Insert(Currents.Instance);
            }
        }
    }
}
