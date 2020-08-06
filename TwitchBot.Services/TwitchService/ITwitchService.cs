using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TwitchBot.Services.TwitchService
{
    /// <summary>
    /// Inteface responsible for handling <see cref="TwitchService"/> functionalities.
    /// </summary>
    public interface ITwitchService
    {
        string Username { get; set; }
        string Password { get; set; }
        TcpClient TCPClient { get; set; }
        StreamReader Reader { get; set; }
        StreamWriter Writer { get; set; }
        
        Task ReadMessagesAsync(string channel);
        /// <summary>
        /// Event that handles how messages are received.
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Connect to twitch service with provided username and password.
        /// </summary>
        /// <param name="channel">Channel for the bot to write messages and/or moderate.</param>
        /// <remarks>Passwords meaning your provided token.</remarks>
        /// <returns>True whether could connect otherwise false.</returns>
        Task ConnectAsync(string channel);

        /// <summary>
        /// Reconnect to twitch service with provided username/password/channel.
        /// </summary>
        /// <param name="channel">Channel for the bot to write messages and/or moderate.</param>
        /// <remarks>Passwords meaning your provided token.</remarks>
        Task ReconnectAsync(string channel);

        /// <summary>
        /// Disconnects bot from a twitch channel.
        /// </summary>
        /// <param name="channel">Channel name.</param>
        void Disconnect(string channel);

        /// <summary>
        /// Write a message to twitch chat.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <param name="channel">Channel to send message.</param>
        Task SendMessageAsync(string message, string channel);
    }
}
