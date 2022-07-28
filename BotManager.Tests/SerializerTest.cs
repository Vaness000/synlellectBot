using BotManager.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotManager.Tests
{
    [TestFixture]
    public class SerializerTest
    {
        [Test]
        public void SerializeTest_GroupList_Success()
        {
            GroupList.Create(new string[] { "user1", "user2"});

            Serializer<GroupList>.Serialize(GroupList.Instance);
            GroupList deserialized = Serializer<GroupList>.Deserialize();

            CollectionAssert.AreEqual(GroupList.Instance.Groups, deserialized.Groups);
        }

        [Test]
        public void SerializeTest_ReviewerList_Success()
        {
            ReviewersList.Create(new List<Reviewer>
            {
                new Reviewer("Иван Р", "fdf"),
                new Reviewer("Иван Р1", "fdf1"),
            });

            Serializer<ReviewersList>.Serialize(ReviewersList.Instance);
            ReviewersList deserialized = Serializer<ReviewersList>.Deserialize();

            CollectionAssert.AreEqual(ReviewersList.Instance.Reviewers, deserialized.Reviewers);
        }
    }
}
