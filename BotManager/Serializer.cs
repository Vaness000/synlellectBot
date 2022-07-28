using BotManager.Logs;
using System;
using System.IO;
using System.Xml.Serialization;

namespace BotManager
{
    public class Serializer<T> where T : new()
    {
        public static void Serialize(T value)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                string fileName = GetFileName(typeof(T));
                File.WriteAllText(fileName, string.Empty);

                using (FileStream fs = new FileStream(Path.Combine(fileName), FileMode.OpenOrCreate))
                {
                    serializer.Serialize(fs, value);
                }
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }

        public static T Deserialize()
        {
            T result = default;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                string fileName = GetFileName(typeof(T));

                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    result = (T)serializer.Deserialize(fs);
                }
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }

            return result;
        }

        private static string GetFileName(Type currentType)
        {
            string fileName = $"{currentType.Name.Replace(".", string.Empty)}.xml";
            return Path.Combine(Environment.CurrentDirectory, fileName);
        }
    }
}
