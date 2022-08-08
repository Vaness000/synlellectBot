using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotManager.Entities
{
    public class UserGroupsList
    {
        public static UserGroupsList Instance { get; private set; }
        public List<UserGroup> UserGroups { get; set; }
        public UserGroupsList()
        {
            UserGroups = new List<UserGroup>();
        }

        public static UserGroupsList Create()
        {
            if(Instance == null)
            {
                Instance = DataProvider.Instance.Get<UserGroupsList>();
            }

            return Instance;
        }

        public bool AddUserToGroup(long chatId, string userName, string groupName)
        {
            UserGroup userGroup = UserGroups.FirstOrDefault(x => x.ChatId == chatId && x.GroupName == groupName && x.UserName == userName);
            if(userGroup == null)
            {
                UserGroups.Add(new UserGroup()
                {
                    ChatId = chatId,
                    UserName = userName,
                    GroupName = groupName
                });

                return true;
            }

            return false;
        }

        public bool RemoveUserFromGroup(long chatId, string userName, string groupName)
        {
            UserGroup userGroup = UserGroups.FirstOrDefault(x => x.ChatId == chatId && x.GroupName == groupName && x.UserName == userName);
            if (userGroup != null)
            {
                return UserGroups.Remove(userGroup);
            }

            return false;
        }
    }
}
