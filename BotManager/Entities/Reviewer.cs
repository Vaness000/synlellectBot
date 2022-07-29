using System;
using System.Collections;
using System.Collections.Generic;

namespace BotManager.Entities
{
    public class Reviewer
    {
        public List<string> Groups { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool IsAvailable { get; set; }
        public string UnavailableReason { get; set; }
        public HashSet<long> Chats { get; set; }

        public Reviewer(string fullName, string userName, bool isAvailable = true)
        {
            this.FullName = fullName;
            this.IsAvailable = isAvailable;
            this.UserName = userName;
            this.UnavailableReason = string.Empty;
            Groups = new List<string>()
            {
                GroupList.DefaultGroupName
            };

            Chats = new HashSet<long>();
        }

        public Reviewer()
        {

        }

        public override string ToString()
        {
            string availability = IsAvailable ? "Доступен" : $"Недоступен - {UnavailableReason}";
            string groups = $" состоит в группах: {string.Join(", ", Groups)}";
            return $"{FullName} - @{UserName} {groups}: {availability}";
        }

        public override bool Equals(object obj)
        {
            Reviewer reviewer = obj as Reviewer;
            if (ReferenceEquals(reviewer, null))
            {
                return false;
            }

            return UserName == reviewer.UserName;
        }
    }
}
