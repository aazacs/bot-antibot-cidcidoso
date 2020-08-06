using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchBot.Application.Services;
using TwitchBot.Services.TwitchService;

namespace TwitchBot.ConsoleApp
{
    partial class Program
    {
        public static bool connected = false;
        private static ITwitchService twitchService;

        private static IMessageHandler messageHandler;
        private static ServiceProvider ServiceProvider { get; set; }

        public static ServiceProvider ConfigureCollection()
        {
            var collection = new ServiceCollection();

            collection.AddScoped<ITwitchService, TwitchService>();
            collection.AddScoped<IMessageHandler, MessageHandler>();

            collection.BuildServiceProvider();
            return collection.BuildServiceProvider();
        }

        public static void ConfigureMessageHandlers()
        {
            twitchService.MessageReceived += messageHandler.PongMessageReceivedHandler;
            twitchService.MessageReceived += Service_MessageReceived;
        }

        public static void ConfigureProviders()
        {
            twitchService = ServiceProvider.GetService<ITwitchService>();
            messageHandler = ServiceProvider.GetService<IMessageHandler>();
        }
    }
}
