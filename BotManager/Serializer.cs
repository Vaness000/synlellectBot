using BotManager.Logs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotManager
{
    public static class Serializer<T> where T : class
    {
        public static void Serialize(T obj)
        {
            string fileName = GetFileName(typeof(T));
            try
            {
                string json = JsonConvert.SerializeObject(obj);
                File.WriteAllText(fileName, json, Encoding.UTF8);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
            }
        }

        public static T Deserialize()
        {
            T result = default;
            string fileName = GetFileName(typeof(T));

            try
            {
                string json = File.ReadAllText(fileName, Encoding.UTF8);
                result = JsonConvert.DeserializeObject<T>(json);
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
                return result;
            }

            return result;
        }

        private static string GetFileName(Type type)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{type.Name}.json");
        }
    }
}
