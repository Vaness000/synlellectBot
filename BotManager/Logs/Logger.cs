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
        }
    }
}
