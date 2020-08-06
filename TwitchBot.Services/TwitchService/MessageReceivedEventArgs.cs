using System;
using System.Collections.Generic;
using System.Text;

namespace TwitchBot.Services.TwitchService
{
    /// <summary>
    /// Message received event arguments.
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.FromSystemFormat;
        }

        /// <summary>
        /// Constructor responsible for initializing a <see cref="MessageReceivedEventArgs"/>.
        /// </summary>
        /// <param name="msg">System received message.</param>
        public MessageReceivedEventArgs(string msg, string channel)
        {
            this.Channel = channel;
            this.FromSystemFormat = msg;

            if (msg.Contains("PRIVMSG"))
            {
                IsUserMessage = true;
                int userLength = Math.Abs(msg.IndexOf('@') + 1 - (msg.IndexOf(".tmi") + 1));
                UsrMessage = new UserMessage
                {
                    User = msg.Substring(msg.IndexOf('@'), userLength),
                    // +3 cause (space, ':' and length +1)
                    Message = msg.Substring(msg.IndexOf('#') + Channel.Length + 3)
                };
            }
        }

        public string Channel { get; private set; }
        public string FromSystemFormat { get; private set; }
        public bool IsUserMessage { get; private set; }
        public UserMessage UsrMessage { get; private set; }

        /// <summary>
        /// Struct responsible for transforming a system sent message into a composed user message.
        /// </summary>
        public struct UserMessage
        {
            public string Message { get; set; }
            public string User { get; set; }
        }
    }
}
