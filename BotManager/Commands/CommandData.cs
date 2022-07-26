using System;
using System.Collections.Generic;
using System.Text;

namespace BotManager.Commands
{
    public class CommandData
    {
        /// <summary>
        /// Information about message sender
        /// </summary>
        public class SenderInfo
        {
            public string UserName { get; set; }
            public string FullName { get; set; }

            public override bool Equals(object obj)
            {
                SenderInfo info = obj as SenderInfo;
                if (ReferenceEquals(info, null))
                {
                    return false;
                }

                return UserName == info.UserName && FullName == info.FullName;
            }
        }
        /// <summary>
        /// Command name
        /// </summary>
        public string CommandKey { get; set; }
        /// <summary>
        /// Telegram nickname
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Sender's Telegram inckname
        /// </summary>
        public SenderInfo Sender { get; set; }
        /// <summary>
        /// Field that can contain additional string data. For example: full name or user's remove reason
        /// </summary>
        public string AdditionalInfo { get; set; }

        public override bool Equals(object obj)
        {
            CommandData data = obj as CommandData;
            if (ReferenceEquals(data, null))
            {
                return false;
            }

            return UserName == data.UserName && AdditionalInfo == data.AdditionalInfo && CommandKey == data.CommandKey && Sender.Equals(data.Sender);
        }
    }
}
