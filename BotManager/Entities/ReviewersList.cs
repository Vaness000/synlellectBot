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
        private List<Reviewer> reviewers;

        public static ReviewersList Instance
        {
            get
            {
                return instatce;
            }
        }

        public IEnumerable<Reviewer> GetReviewers
        {
            get
            {
                foreach(Reviewer reviewer in this.reviewers)
                {
                    yield return reviewer;
                }
            }
        }

        private ReviewersList(IEnumerable<Reviewer> reviewers)
        {
            this.reviewers = new List<Reviewer>();
            foreach (Reviewer reviewer in reviewers)
            {
                if (GetReviewer(reviewer.UserName) == null)
                {
                    this.reviewers.Add(reviewer);
                }
            }

            current = 0;
        }

        public static ReviewersList Create(IEnumerable<Reviewer> reviewers)
        {
            if(instatce == null)
            {
                instatce = new ReviewersList(reviewers);
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

        public bool AddReviewer(string userName, string fullName)
        {
            if(GetReviewer(userName) != null)
            {
                return false;
            }

            reviewers.Add(new Reviewer(fullName, userName));
            return true;
        }

        private IEnumerable<Reviewer> GetAvailableReviewers(string sender, string group = GroupList.DefaultGroupName)
        {
            return reviewers.Where(x => x.IsAvailable && x.UserName != sender && x.Groups.Contains(group));
        }

        public Reviewer GetReviewerToCheckTask(string sender, string group = GroupList.DefaultGroupName)
        {
            var availableReviewers = GetAvailableReviewers(sender, group).ToList();
            MoveNext(availableReviewers.Count());
            return availableReviewers.Count > 0 ? availableReviewers.ElementAt(current) : null;
        }

        public bool RemoveReviewer(string userName, bool isPermanently = false, string suspendReason = null)
        {
            Reviewer reviewerToRemove = GetReviewer(userName);

            if (reviewerToRemove == null)
            {
                return false;
            }

            if (isPermanently)
            {
                reviewers.Remove(reviewerToRemove);
            }
            else
            {
                reviewerToRemove.IsAvailable = false;
                reviewerToRemove.UnavailableReason = suspendReason;
            }

            return true;
        }

        public bool RecoverReviewer(string userName)
        {
            Reviewer reviewer = reviewers.FirstOrDefault(x => x.UserName == userName && !x.IsAvailable);

            if (reviewer != null && !reviewer.IsAvailable)
            {
                reviewer.IsAvailable = true;
                reviewer.UnavailableReason = string.Empty;
                return true;
            }

            return false;
        }

        public Reviewer GetReviewer(string userName)
        {
            return reviewers.FirstOrDefault(x => x.UserName == userName);
        }
    }
}
