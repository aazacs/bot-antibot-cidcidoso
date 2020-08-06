using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TwitchBot.Services.TwitchService
{
    public static class Extensions
    {
        public async static Task WriteLineAndFlushAsync(this StreamWriter sw, string message)
        {
            await sw.WriteLineAsync(message);
            await sw.FlushAsync();
        }
    }

    /// <summary>
    /// Class responsible for handling functionalities related to the twitch service.
    /// </summary>
    public class TwitchService : ITwitchService
    {
        public TcpClient TCPClient { get; set; }
        public StreamReader Reader { get; set; }
        public StreamWriter Writer { get; set; }

        /// <summary>
        /// Twitch service host.
        /// </summary>
        public const string TWITCH_HOST = "irc.chat.twitch.tv";
        /// <summary>
        /// Twitch service port.
        /// </summary>
        public const int TWITCH_PORT = 6667;

        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Constructor to initialize threads.
        /// </summary>
        public TwitchService()
        {
        }

        /// <summary>
        /// Reads incoming messages from the chat.
        /// </summary>
        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Handlers received messages. Must use  custom implementation.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Connect to twitch service with provided username and password.
        /// </summary>
        /// <param name="channel">Channel for the bot to write messages and/or moderate.</param>
        /// <remarks>Passwords meaning your provided token.</remarks>
        /// <returns>True whether could connect otherwise false.</returns>
        public async Task ConnectAsync(string channel)
        {
            try
            {
                TCPClient = new TcpClient(TWITCH_HOST, TWITCH_PORT);
                Reader = new StreamReader(TCPClient.GetStream());
                Writer = new StreamWriter(TCPClient.GetStream());

                // logs in
                await Writer.WriteLineAndFlushAsync(
                        "PASS " + Password + Environment.NewLine +
                        "NICK " + Username + Environment.NewLine +
                        "USER " + Username + " 8 * :" + Username
                    );

                // shows how many users are logged on chat, shows online mods
                await Writer.WriteLineAndFlushAsync(
                    "CAP REQ :twitch.tv/membership"
                );
                // join chatroom
                await Writer.WriteLineAndFlushAsync(
                    $"JOIN #{channel.ToLower()}"
                );

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Reconnect to twitch service with provided username/password/channel.
        /// </summary>
        /// <param name="channel">Channel for the bot to write messages and/or moderate.</param>
        /// <remarks>Passwords meaning your provided token.</remarks>
        public async Task ReconnectAsync(string channel)
        {
            Disconnect(channel);
            await ConnectAsync(channel);
        }

        /// <summary>
        /// Disconnects bot from a twitch channel.
        /// </summary>
        /// <param name="channel">Channel name.</param>
        public void Disconnect(string channel)
        {
            Reader.Dispose();
            Writer.Dispose();
            TCPClient.Dispose();
        }

        /// <summary>
        /// Write a message to twitch chat.
        /// </summary>
        /// <param name="message">Message to send.</param>
        public async Task SendMessageAsync(string message, string channel)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                var msgFormat =
                    $":{Username.ToLower()}!{Username.ToLower()}@{Username.ToLower()}.tmi.twitch.tv PRIVMSG #{channel.ToLower()} :{message}";

                await Writer.WriteLineAndFlushAsync(msgFormat);
            }
        }

        public async Task ReadMessagesAsync(string channel)
        {
            if (TCPClient.Available > 0 || Reader.Peek() >= 0)
            {
                var message = await Reader.ReadLineAsync();
                OnMessageReceived(new MessageReceivedEventArgs(message, channel));
            }
        }
    }
}
