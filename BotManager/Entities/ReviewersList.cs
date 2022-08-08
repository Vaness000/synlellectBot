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
        }

        public ReviewersList()
        {

        }

        public static ReviewersList Create(IEnumerable<Reviewer> reviewers = null)
        {
            if(instatce == null)
            {
                instatce = reviewers != null ? new ReviewersList(reviewers) : DataProvider.Instance.Get<ReviewersList>();
            }

            return instatce;
        }

        private int MoveNext(int availableCount, long chat)
        {
            if(Currents.Instance.CurrentReviewerIndexes.TryGetValue(chat, out int current))
            {
                Currents.Instance.CurrentReviewerIndexes[chat] = availableCount <= current + 1 ? 0 : current + 1;
            }
            else
            {
                Currents.Instance.CurrentReviewerIndexes.Add(chat, current + 1);
            }

            return current;
        }

        public bool AddReviewer(string userName, string fullName, long chat)
        {
            Reviewer reviewer = GetReviewer(userName, chat);
            if(reviewer != null)
            {
                return reviewer.Chats.Add(chat);
            }

            reviewer = GetReviewer(userName);
            if(reviewer == null)
            {
                reviewer = new Reviewer(fullName, userName);
                UserGroupsList.Instance.AddUserToGroup(chat, userName, GroupList.DefaultGroupName);
                Reviewers.Add(reviewer);
            }
            
            reviewer.Chats.Add(chat);

            return true;
        }

        private IEnumerable<Reviewer> GetAvailableReviewers(string sender, long chat, string group = GroupList.DefaultGroupName)
        {
            return UserGroupsList.Instance.UserGroups.Where(x => x.GroupName == group && x.ChatId == chat && x.UserName != sender)
                                                     .Select(x => GetReviewer(x.UserName))
                                                     .Where(x => x.IsAvailable);
        }

        public Reviewer GetReviewerToCheckTask(string sender, long chat, string group = GroupList.DefaultGroupName)
        {
            var availableReviewers = GetAvailableReviewers(sender, chat, group);
            int position = MoveNext(availableReviewers.Count(), chat);
            return availableReviewers.Count() > 0 ? availableReviewers.ElementAt(position) : null;
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
                UserGroupsList.Instance.UserGroups = UserGroupsList.Instance.UserGroups.Where(x => x.ChatId != chat || x.UserName != userName).ToList();

                if (reviewerToRemove.Chats.Count == 0)
                {
                    Reviewers.Remove(reviewerToRemove);
                }
            }
            else
            {
                if (reviewerToRemove.IsAvailable)
                {
                    reviewerToRemove.IsAvailable = false;
                    reviewerToRemove.UnavailableReason = suspendReason;
                }
                else
                {
                    return false;
                }
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
