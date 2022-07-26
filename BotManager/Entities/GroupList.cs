using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public List<string> Groups { get; private set; }

        private GroupList(IEnumerable<string> groups = null)
        {
            Groups = new List<string>()
            {
                DefaultGroupName
            };

            if(groups != null)
            {
                foreach(string group in groups)
                {
                    if (!Groups.Contains(group))
                    {
                        Groups.Add(group);
                    }
                }
            }
        }

        public string GetGroup(string name)
        {
            return Groups.FirstOrDefault(x => x == name);
        } 

        public static GroupList Create(IEnumerable<string> groups = null)
        {
            if(instance == null)
            {
                instance = new GroupList(groups);
            }

            return instance;
        }

        public bool AddGroup(string name)
        {
            bool result = false;

            if(GetGroup(name) == null)
            {
                Groups.Add(name);
                result = true;
            }

            return result;
        }

        public bool RemoveGroup(string name)
        {
            bool result = false;
            string group = GetGroup(name);
            if (group != null)
            {
                Groups.Remove(group);

                foreach(Reviewer reviewer in ReviewersList.Instance.GetReviewers.Where(x => x.Groups.Contains(name)))
                {
                    reviewer.Groups.Remove(name);
                }

                result = true;
            }

            return result;
        }

        public bool AddReviewerToGroup(string userName, string group)
        {
            bool result = false;

            Reviewer reviewer = ReviewersList.Instance.GetReviewer(userName);
            group = GetGroup(group);

            if(reviewer != null && group != null && !reviewer.Groups.Contains(group))
            {
                reviewer.Groups.Add(group);
                result = true;
            }

            return result;
        }

        public bool RemoveFromGroup(string userName, string groupName)
        {
            bool result = false;

            Reviewer reviewer = ReviewersList.Instance.GetReviewer(userName);
            groupName = GetGroup(groupName);

            if (reviewer != null && groupName != null && reviewer.Groups.Contains(groupName))
            {
                reviewer.Groups.Remove(groupName);
                result = true;
            }

            return result;
        }
    }
}
