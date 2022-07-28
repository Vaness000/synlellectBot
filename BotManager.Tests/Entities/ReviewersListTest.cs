using BotManager.Entities;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotManager.Tests.Entities
{
    [TestFixture]
    public class ReviewersListTest
    {
        private static IEnumerable<string> GetReviewers()
        {
            yield return "Иван Рыжаев";
            yield return "Александр Алексеев";
            yield return "Петр Петров";
            yield return "Алексей Иванов";
            yield return "Илья Размахнин";
            yield return "Мария Соколова";
        }

        [Test]
        public void TestCreate_ListNames_SingletonCreated()
        {
            IEnumerable<Reviewer> reviewers1 = GetReviewers().Select(x => new Reviewer($"{x}", x));
            IEnumerable<Reviewer> reviewers2 = GetReviewers().Skip(2).Select(x => new Reviewer($"{x}", x));

            ReviewersList reviewersList1 = ReviewersList.Create(reviewers1);
            ReviewersList reviewersList2 = ReviewersList.Create(reviewers2);

            Assert.IsNotNull(ReviewersList.Instance);
            CollectionAssert.AreEqual(reviewersList1.Reviewers, reviewersList2.Reviewers);
            CollectionAssert.AreEqual(ReviewersList.Instance.Reviewers, reviewersList1.Reviewers);
            CollectionAssert.AreEqual(ReviewersList.Instance.Reviewers, reviewers1);
            CollectionAssert.AreNotEqual(ReviewersList.Instance.Reviewers, reviewers2);
        }

        [Test]
        [TestCaseSource(nameof(GetTestAddReviewerCases))]
        public bool TestAddReviewer(string reviewer)
        {
            IEnumerable<Reviewer> reviewers = GetReviewers().Select(x => new Reviewer($"{x}", x));
            ReviewersList.Create(reviewers);

            return ReviewersList.Instance.AddReviewer($"@{reviewer}", reviewer);
        }

        private static IEnumerable GetTestAddReviewerCases()
        {
            yield return new TestCaseData("Виктор Макаров").Returns(true);
            yield return new TestCaseData("Виктор Марков").Returns(true);
            yield return new TestCaseData("Макр Викторов").Returns(true);

            yield return new TestCaseData("Илья Размахнин").Returns(false);
            yield return new TestCaseData("Мария Соколова").Returns(false);
        }

        [Test]
        [TestCaseSource(nameof(GetTestRemoveReviewerCases))]
        public bool TestRemoveReviewer(string userName, bool isPermanently)
        {
            IEnumerable<Reviewer> reviewers = GetReviewers().Select(x => new Reviewer($"{x}", x));
            ReviewersList.Create(reviewers);

            return ReviewersList.Instance.RemoveReviewer(userName, isPermanently);
        }

        private static IEnumerable GetTestRemoveReviewerCases()
        {
            yield return new TestCaseData("@Виктор Макаров", true).Returns(false);
            yield return new TestCaseData("@Виктор Марков", true).Returns(false);
            yield return new TestCaseData("@Макр Викторов", true).Returns(false);

            yield return new TestCaseData("@Илья Размахнин", true).Returns(true);
            yield return new TestCaseData("@Мария Соколова", true).Returns(true);

            yield return new TestCaseData("@Виктор Макаров", false).Returns(false);
            yield return new TestCaseData("@Виктор Марков", false).Returns(false);
            yield return new TestCaseData("@Макр Викторов", false).Returns(false);

            yield return new TestCaseData("@Иван Рыжаев", false).Returns(true);
            yield return new TestCaseData("@Александр Алексеев", false).Returns(true);
        }

        [Test]
        [TestCaseSource(nameof(GetTestGerReviewerData))]
        public Reviewer TestGetReviewer(string userName)
        {
            IEnumerable<Reviewer> reviewers = GetReviewers().Select(x => new Reviewer($"{x}", x));
            ReviewersList.Create(reviewers);

            return ReviewersList.Instance.GetReviewer(userName);
        }

        private static IEnumerable GetTestGerReviewerData()
        {
            yield return new TestCaseData("@Иван Рыжаев").Returns(new Reviewer("@Иван Рыжаев", "Иван Рыжаев"));
            yield return new TestCaseData("@Александр Алексеев").Returns(new Reviewer(@"Александр Алексеев", "Александр Алексеев"));
            yield return new TestCaseData("@Петр Петров").Returns(new Reviewer("@Петр Петров", "Петр Петров"));
            yield return new TestCaseData("@Алексей Иванов").Returns(new Reviewer("@Алексей Иванов", "Алексей Иванов"));
            yield return new TestCaseData("@Илья Размахнин").Returns(new Reviewer("@Илья Размахнин", "Илья Размахнин"));
            yield return new TestCaseData("@Мария Соколова").Returns(new Reviewer("@Мария Соколова", "Мария Соколова"));

            yield return new TestCaseData("Мария Соколова1").Returns(null);
            yield return new TestCaseData("1Мария Соколова").Returns(null);
            yield return new TestCaseData("Рыжаев Иван").Returns(null);
        }

        [Test]
        [TestCaseSource(nameof(GetTestGetUserToCheckCases))]
        public bool TestGetReviewerToCheck(string sender)
        {
            ReviewersList.Create(GetReviewersToTest());

            Reviewer reviewer = ReviewersList.Instance.GetReviewerToCheckTask(sender);

            return reviewer.UserName != sender && reviewer.IsAvailable;
        }
        private static IEnumerable<Reviewer> GetReviewersToTest()
        {
            yield return new Reviewer("@Иван Рыжаев", "Иван Рыжаев");
            yield return new Reviewer("@Илья Размахнин", "Илья Размахнин");
            yield return new Reviewer("@Семен Сидоров", "Семен Сидоров", false);
            yield return new Reviewer("@Петр Ручкин", "Петр Ручкин");
            yield return new Reviewer("@Кирилл Жуков", "Кирилл Жуков", false);
        }
        private static IEnumerable GetTestGetUserToCheckCases()
        {
            yield return new TestCaseData("@Иван Рыжаев").Returns(true);
            yield return new TestCaseData("@Илья Размахнин").Returns(true);
            yield return new TestCaseData("@Семен Сидоров").Returns(true);
            yield return new TestCaseData("@Петр Ручкин").Returns(true);
            yield return new TestCaseData("@Кирилл Жуков").Returns(true);

            yield return new TestCaseData("@Светлана Кирова").Returns(true);
            yield return new TestCaseData("@Ольга Щербакова").Returns(true);
        }

        [Test]
        [TestCaseSource(nameof(GetTestRecoverUsersCases))]
        public bool TestRecoverUser(string userName)
        {
            ReviewersList.Create(GetReviewersToTest());

            return ReviewersList.Instance.RecoverReviewer(userName);
        }

        private static IEnumerable GetTestRecoverUsersCases()
        {
            yield return new TestCaseData("@Иван Рыжаев").Returns(false);
            yield return new TestCaseData("@Илья Размахнин").Returns(false);
            yield return new TestCaseData("@Семен Сидоров").Returns(true);
            yield return new TestCaseData("@Петр Ручкин").Returns(false);
            yield return new TestCaseData("@Кирилл Жуков").Returns(true);
            yield return new TestCaseData("@Светлана Кирова").Returns(false);
            yield return new TestCaseData("@Ольга Щербакова").Returns(false);
        }
    }

}
