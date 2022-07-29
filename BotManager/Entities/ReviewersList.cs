using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotManager.Entities
{
    public class ReviewersList
    {
        private static ReviewersList instatce;

        private int current;

        //just for serialization
        public List<Reviewer> Reviewers { get; set; }

        public static ReviewersList Instance
        {
            get
            {
                return instatce;
            }
        }

        [JsonIgnore]
        public IEnumerable<Reviewer> GetReviewers
        {
            get
            {
                foreach(Reviewer reviewer in this.Reviewers)
                {
                    yield return reviewer;
                }
            }
        }

        private ReviewersList(IEnumerable<Reviewer> reviewers)
        {
            this.Reviewers = new List<Reviewer>();
            foreach (Reviewer reviewer in reviewers)
            {
                if (GetReviewer(reviewer.UserName) == null)
                {
                    this.Reviewers.Add(reviewer);
                }
            }

            current = 0;
        }

        public ReviewersList()
        {

        }

        public static ReviewersList Create(IEnumerable<Reviewer> reviewers = null)
        {
            if(instatce == null)
            {
                instatce = reviewers != null ? new ReviewersList(reviewers) : Serializer<ReviewersList>.Deserialize();
            }

            return instatce;
        }

        private void MoveNext(int availableCount)
        {
            current++;

            if (current >= availableCount)
            {
                current = 0;
            }
        }

        public bool AddReviewer(string userName, string fullName, long chat)
        {
            Reviewer reviewer = GetReviewer(userName, chat);
            if(reviewer != null)
            {
                return reviewer.Chats.Add(chat);
            }

            reviewer = new Reviewer(fullName, userName);
            reviewer.Chats.Add(chat);
            Reviewers.Add(reviewer);

            return true;
        }

        private IEnumerable<Reviewer> GetAvailableReviewers(string sender, long chat, string group = GroupList.DefaultGroupName)
        {
            return Reviewers.Where(x => x.IsAvailable && x.UserName != sender && x.Groups.Contains(group) && x.Chats.Contains(chat));
        }

        public Reviewer GetReviewerToCheckTask(string sender, long chat, string group = GroupList.DefaultGroupName)
        {
            var availableReviewers = GetAvailableReviewers(sender, chat, group).ToList();
            MoveNext(availableReviewers.Count());
            return availableReviewers.Count > 0 ? availableReviewers.ElementAt(current) : null;
        }

        public bool RemoveReviewer(string userName, long chat, bool isPermanently = false, string suspendReason = null)
        {
            Reviewer reviewerToRemove = GetReviewer(userName, chat);

            if (reviewerToRemove == null)
            {
                return false;
            }

            if (isPermanently)
            {
                reviewerToRemove.Chats.Remove(chat);
                if(reviewerToRemove.Chats.Count == 0)
                {
                    Reviewers.Remove(reviewerToRemove);
                }
            }
            else
            {
                reviewerToRemove.IsAvailable = false;
                reviewerToRemove.UnavailableReason = suspendReason;
            }

            return true;
        }

        public bool RecoverReviewer(string userName, long chat)
        {
            Reviewer reviewer = Reviewers.FirstOrDefault(x => x.UserName == userName && !x.IsAvailable && x.Chats.Contains(chat));

            if (reviewer != null)
            {
                reviewer.IsAvailable = true;
                reviewer.UnavailableReason = string.Empty;
                return true;
            }

            return false;
        }

        public Reviewer GetReviewer(string userName)
        {
            return Reviewers.FirstOrDefault(x => x.UserName == userName);
        }

        public Reviewer GetReviewer(string userName, long chat)
        {
            return Reviewers.FirstOrDefault(x => x.UserName == userName && x.Chats.Contains(chat));
        }
    }
}
