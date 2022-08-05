using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

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

                if(group.Chats.Count == 0)
                {
                    Groups.Remove(group);
                }

                foreach(Reviewer reviewer in ReviewersList.Instance.GetReviewers.Where(x => x.Groups.Contains(name)))
                {
                    reviewer.Groups.Remove(name);
                }

                result = true;
            }

            return result;
        }

        public bool AddReviewerToGroup(string userName, string groupName, long chat)
        {
            bool result = false;

            Reviewer reviewer = ReviewersList.Instance.GetReviewer(userName, chat);
            Group group = GetGroup(groupName, chat);

            if(reviewer != null && group != null && !reviewer.Groups.Contains(group.Name))
            {
                reviewer.Groups.Add(group.Name);
                result = true;
            }

            return result;
        }

        public bool RemoveFromGroup(string userName, string groupName, long chat)
        {
            bool result = false;

            Reviewer reviewer = ReviewersList.Instance.GetReviewer(userName, chat);
            Group group = GetGroup(groupName, chat);

            if (reviewer != null && groupName != null && reviewer.Groups.Contains(group.Name))
            {
                reviewer.Groups.Remove(groupName);
                result = true;
            }

            return result;
        }
    }
}
