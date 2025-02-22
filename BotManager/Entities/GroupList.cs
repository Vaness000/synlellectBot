﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BotManager.Entities
{
    public class GroupList
    {
        public const string DefaultGroupName = "review";
        private static GroupList instance;
        public static GroupList Instance
        {
            get
            {
                return instance;
            }
        }

        public List<Group> Groups { get; set; }

        private GroupList(IEnumerable<Group> groups = null)
        {
            Groups = new List<Group>()
            {
                new Group(DefaultGroupName)
            };

            if(groups != null)
            {
                foreach(var group in groups)
                {
                    if (!Groups.Contains(group))
                    {
                        Groups.Add(group);
                    }
                }
            }
        }

        [JsonConstructor]
        public GroupList() { }

        public Group GetGroup(string name, long chat)
        {
            return Groups.FirstOrDefault(x => x.Name == name && x.Chats.Contains(chat));
        } 

        public Group GetGroup(string name)
        {
            return Groups.FirstOrDefault(x => x.Name == name);
        }

        public static GroupList Create(IEnumerable<Group> groups = null)
        {
            if(instance == null)
            {
                instance = groups != null ? new GroupList(groups) : DataProvider.Instance.Get<GroupList>();
            }

            return instance;
        }

        public bool AddGroup(string name, long chat)
        {
            Group group = GetGroup(name);
            if (group == null)
            {
                group = new Group(name);
                Groups.Add(group);
            }

            return group.Chats.Add(chat);
        }

        public bool RemoveGroup(string name, long chat)
        {
            bool result = false;
            Group group = GetGroup(name);
            if (group != null)
            {
                group.Chats.Remove(chat);

                UserGroupsList.Instance.UserGroups = UserGroupsList.Instance.UserGroups.Where(x => x.ChatId != chat || x.GroupName != name).ToList();
                if(group.Chats.Count == 0)
                {
                    Groups.Remove(group);
                }

                result = true;
            }

            return result;
        }
    }
}
