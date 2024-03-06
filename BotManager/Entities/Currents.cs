using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotManager.Entities
{
    public class Currents
    {
        public List<UserGroupIndexes> CurrentReviewerIndexes { get; set; }
        public static Currents Instance { get; private set; }

        public static Currents Create()
        {
            if (Instance == null)
            {
                Instance = DataProvider.Instance.Get<Currents>();
            }

            return Instance;
        }

        public Currents()
        {

        }
    }
}
