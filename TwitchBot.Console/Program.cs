using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TwitchBot.Application.Services;
using TwitchBot.Services.TwitchService;

namespace TwitchBot.ConsoleApp
{
    partial class Program
    {
        public static string channel = "aazacs";

        static async Task Main(string[] args)
        {
            ServiceProvider = ConfigureCollection();
            ConfigureProviders();
            ConfigureMessageHandlers();

            string username, password;

            Console.WriteLine("Twitch username:");
            username = Console.ReadLine();

            Console.WriteLine("Twitch OAUTH password:");
            password = Console.ReadLine();

            Console.WriteLine("Channel to connect:");
            channel = Console.ReadLine();

            twitchService.Username = username;
            twitchService.Password = password;

            await twitchService.ConnectAsync(channel);
            Console.WriteLine("[*] Awaiting connection...");
            while (true)
            {
                await twitchService.ReadMessagesAsync(channel);
            }
        }

        private static bool IsBot(string username)
        {
            if (username[0].Equals('@'))
                username = username.Substring(1, username.Length-1);
                
            string guidValue = username.PadRight(32, '0');
            var isBot = Guid.TryParseExact(guidValue, "N", out _);
            if (isBot)
                Console.WriteLine("ban");
            else
                Console.WriteLine("safe");
            return isBot;
        }

        private static void Service_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.IsUserMessage)
            {
                Console.WriteLine($"[*] {e.UsrMessage.User}: {e.UsrMessage.Message}");
                if (IsBot(e.UsrMessage.User))
                {
                    Task.Run(async () => await twitchService.SendMessageAsync($"/ban {e.UsrMessage.User} BOT", channel));
                }
            }
            else
            {
                Console.WriteLine($"[*] {e.ToString()}");
            }

            if (e.FromSystemFormat.Contains("366", StringComparison.OrdinalIgnoreCase))
            {
                connected = true;
                Console.WriteLine("[*] Connected");
            }
        }
    }
}
