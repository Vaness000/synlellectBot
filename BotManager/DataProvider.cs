using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using BotManager.Logs;

namespace BotManager
{
    public class DataProvider
    {
        private IFirebaseConfig config;
        private IFirebaseClient firebaseClient;

        public static DataProvider Instance { get; private set; }
        private DataProvider()
        {
            config = new FirebaseConfig()
            {
                AuthSecret = Properties.Resources.AuthSecret,
                BasePath = Properties.Resources.BasePath
            };

            firebaseClient = new FireSharp.FirebaseClient(config);
        }

        public static DataProvider Create()
        {
            if (Instance == null)
            {
                Instance = new DataProvider();
            }

            return Instance;
        }

        public bool Insert<T>(T data) where T : new()
        {
            try
            {
                var response = firebaseClient.Set(typeof(T).Name, data);
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
                return false;
            }
        }

        public T Get<T>() where T : new()
        {
            try
            {
                var result = firebaseClient.Get(typeof(T).Name);
                return result.ResultAs<T>();
            }
            catch(Exception e)
            {
                Logger.Log(LogType.Error, e.Message);
                return default;
            }
        }
    }
}
