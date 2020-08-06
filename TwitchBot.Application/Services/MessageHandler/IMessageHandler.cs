using TwitchBot.Services.TwitchService;

namespace TwitchBot.Application.Services
{
    public interface IMessageHandler
    {
        /// <summary>
        /// Handles server ping request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        void PongMessageReceivedHandler(object sender, MessageReceivedEventArgs message);

        /// <summary>
        /// Handles that when people @me I respond.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AtBotMessageReceivedHandler(object sender, MessageReceivedEventArgs e);

        /// <summary>
        /// Handles bot commands.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CommandMessageReceivedHandler(object sender, MessageReceivedEventArgs e);
    }
}
