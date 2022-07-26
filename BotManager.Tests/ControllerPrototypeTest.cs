using BotManager.Commands;
using BotManager.Entities;
using ConsoleDev;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Telegram.Bot.Types;

namespace BotManager.Tests
{
    [TestFixture]
    public class ControllerPrototypeTest
    {
        private ControllerPrototype controller = ControllerPrototype.Create();
        private Message message = new Message()
        {
            From = new User()
            {
                Username = "me",
                FirstName = "Ivan",
                LastName = "Ryzhaev"
            }
        };

        //[Test]
        //[TestCaseSource(nameof(GetParseCommandTestCases))]
        //public bool ParseCommandTest_TypeTest(string messageText, Type commandType)
        //{
        //    message.Text = messageText;

        //    Command command = controller.ParseCommand(message, out CommandData commandData);

        //    return command != null ? command.GetType() == commandType : false;
        //}

        private static IEnumerable GetParseCommandTestCases()
        {
            yield return new TestCaseData("/add", typeof(AddUserCommand)).Returns(true);
            yield return new TestCaseData("add", typeof(AddUserCommand)).Returns(true);
            yield return new TestCaseData("add userName", typeof(AddUserCommand)).Returns(true);
            yield return new TestCaseData("add userName \"FName LName\"", typeof(AddUserCommand)).Returns(true);

            yield return new TestCaseData("/help", typeof(HelpCommand)).Returns(true);
            yield return new TestCaseData("help", typeof(HelpCommand)).Returns(true);

            yield return new TestCaseData("/recover", typeof(RecoverCommand)).Returns(true);
            yield return new TestCaseData("recover", typeof(RecoverCommand)).Returns(true);
            yield return new TestCaseData("recover userName", typeof(RecoverCommand)).Returns(true);

            yield return new TestCaseData("/remove", typeof(RemoveUserCommand)).Returns(true);
            yield return new TestCaseData("remove", typeof(RemoveUserCommand)).Returns(true);
            yield return new TestCaseData("remove userName", typeof(RemoveUserCommand)).Returns(true);

            yield return new TestCaseData("/suspend", typeof(RemoveTemporarlyCommand)).Returns(true);
            yield return new TestCaseData("suspend", typeof(RemoveTemporarlyCommand)).Returns(true);
            yield return new TestCaseData("suspend userName", typeof(RemoveTemporarlyCommand)).Returns(true);

            yield return new TestCaseData("/review", typeof(ReviewCommand)).Returns(true);
            yield return new TestCaseData("review", typeof(ReviewCommand)).Returns(true);

            yield return new TestCaseData("/show", typeof(ShowCommand)).Returns(true);
            yield return new TestCaseData("show", typeof(ShowCommand)).Returns(true);

            yield return new TestCaseData("foo userName \"FName LName\"", typeof(Command)).Returns(false);
            yield return new TestCaseData("/foo", typeof(Command)).Returns(false);
            yield return new TestCaseData(string.Empty, typeof(Command)).Returns(false);
            yield return new TestCaseData(null, typeof(Command)).Returns(false);
        }

        //[Test]
        //[TestCaseSource(nameof(GetFullNameFromMessageTestCases))]
        //public string GetFullNameFromMessageTest(string messageText)
        //{
        //    return Com.GetFullNameFromMessage(messageText);
        //}

        private static IEnumerable GetFullNameFromMessageTestCases()
        {
            yield return new TestCaseData("\"Иван\"").Returns("Иван");
            yield return new TestCaseData("\"Иван Иванов\"").Returns("Иван Иванов");
            yield return new TestCaseData("\"а\"").Returns("а");
            yield return new TestCaseData("\"а а а\"").Returns("а а а");

            yield return new TestCaseData("Иван").Returns(null);
            yield return new TestCaseData("\"Иван").Returns(null);
            yield return new TestCaseData("Иван\"").Returns(null);
            yield return new TestCaseData("").Returns(null);
            yield return new TestCaseData(null).Returns(null);
        }

        [Test]
        [TestCaseSource(nameof(GetCommandDataTestCases))]
        public bool ParseCommandTest_CommandDataTest(string messageText, CommandData expected)
        {
            message.Text = messageText;

            Command command = Command.ParseCommand(message, out CommandData actual);

            return actual != null && expected.Equals(actual);
        }

        private static IEnumerable GetCommandDataTestCases()
        {
            yield return new TestCaseData("/add",
                new CommandData()
                {
                    CommandKey = "/add",
                    Sender = new CommandData.SenderInfo
                    {
                        UserName = "me",
                        FullName = "Ivan Ryzhaev"
                    }
                }).Returns(true);
            yield return new TestCaseData("add",
                new CommandData()
                {
                    CommandKey = "add",
                    Sender = new CommandData.SenderInfo
                    {
                        UserName = "me",
                        FullName = "Ivan Ryzhaev"
                    }
                }).Returns(true);
            yield return new TestCaseData("add userName",
                new CommandData()
                {
                    CommandKey = "add",
                    Sender = new CommandData.SenderInfo
                    {
                        UserName = "me",
                        FullName = "Ivan Ryzhaev"
                    },
                    UserName = "userName"
                }).Returns(true);
            yield return new TestCaseData("add userName \"FName LName\"",
                new CommandData()
                {
                    CommandKey = "add",
                    Sender = new CommandData.SenderInfo
                    {
                        UserName = "me",
                        FullName = "Ivan Ryzhaev"
                    },
                    UserName = "userName",
                    AdditionalInfo = "FName LName"
                }).Returns(true);

            yield return new TestCaseData("suspend userName \"Vacation to 12.02\"",
                new CommandData()
                {
                    CommandKey = "suspend",
                    Sender = new CommandData.SenderInfo
                    {
                        UserName = "me",
                        FullName = "Ivan Ryzhaev"
                    },
                    UserName = "userName",
                    AdditionalInfo = "Vacation to 12.02",
                }).Returns(true);

            yield return new TestCaseData("suspend \"Vacation to 12.02\"",
                new CommandData()
                {
                    CommandKey = "suspend",
                    Sender = new CommandData.SenderInfo
                    {
                        UserName = "me",
                        FullName = "Ivan Ryzhaev"
                    },
                    AdditionalInfo = "Vacation to 12.02",
                }).Returns(true);


            yield return new TestCaseData(string.Empty, null).Returns(false);
            yield return new TestCaseData(null, null).Returns(false);
        }

        [Test]
        public void GetButtonsTest()
        {
            GroupList.Create();
            var buttons = ControllerPrototype.GetButtons();
            foreach(var buttonsRow in buttons)
            {
                var list = buttonsRow.ToList();
            }
        }
    }
}
