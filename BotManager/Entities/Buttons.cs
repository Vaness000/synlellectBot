using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotManager.Entities
{
    public class Buttons
    {
        public static IEnumerable<string> Get
        {
            get
            {
                yield return "show";
                yield return "help";
            }
        }
    }
}
