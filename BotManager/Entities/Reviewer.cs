using System;
using System.Collections;
using System.Collections.Generic;

namespace BotManager.Entities
{
    public class Reviewer
    {
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

            Chats = new HashSet<long>();
        }

        public Reviewer()
        {

        }

        public override string ToString()
        {
            string availability = IsAvailable ? "Доступен" : $"Недоступен - {UnavailableReason}";
            return $"{FullName} - @{UserName}: {availability}";
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
