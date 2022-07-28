using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotManager.Entities
{
    public class Group
    {
        public string Name { get; set; }
        public HashSet<long> Chats { get; set; }
        public Group(string name, IEnumerable<long> chats = null)
        {
            Name = name;
            Chats = new HashSet<long>();
            if(chats != null)
            {
                foreach(int chat in chats)
                {
                    Chats.Add(chat);
                }
            }
        }
    }
}
