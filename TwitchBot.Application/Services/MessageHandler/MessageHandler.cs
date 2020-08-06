using System;
using TwitchBot.Services.TwitchService;

namespace TwitchBot.Application.Services
{
    /// <summary>
    /// Class responsible for handling messages.
    /// </summary>
    public class MessageHandler : IMessageHandler
    {
        /// <summary>
        /// Twitch service.
        /// </summary>
        private readonly ITwitchService service;
        private readonly TimeSpan RespondToAtSpan = new TimeSpan(0, 0, 15);
        /// <summary>
        /// Last time someone @me.
        /// </summary>
        private DateTime LastAtMe { get; set; }

        public MessageHandler(ITwitchService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Handles server ping request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void PongMessageReceivedHandler(object sender, MessageReceivedEventArgs e)
        {
            if (e.FromSystemFormat.Contains("ping", StringComparison.OrdinalIgnoreCase))
                this.service.SendMessageAsync("PONG :tmi.twitch.tv", e.Channel);
        }

        /// <summary>
        /// Handles that when people @me I respond.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AtBotMessageReceivedHandler(object sender, MessageReceivedEventArgs e)
        {
            if (DateTime.Now.ToUniversalTime() > LastAtMe.Add(RespondToAtSpan))
            {
                if (e.IsUserMessage
                    && e?.UsrMessage.Message.Contains(Credentials.Username,StringComparison.OrdinalIgnoreCase) == true)
                {
                    this.service.SendMessageAsync("Don't @me bro! I'm trying to working here. DansGame", e.Channel);
                    LastAtMe = DateTime.Now.ToUniversalTime();
                }
            }
        }

        /// <summary>
        /// Handles bot commands.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CommandMessageReceivedHandler(object sender, MessageReceivedEventArgs e)
        {

        }
    }
}
