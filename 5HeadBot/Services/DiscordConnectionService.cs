﻿using _5HeadBot.DTOS;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace _5HeadBot.Services
{
    public class DiscordConnectionService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly ConfigDTO _config;
        public DiscordConnectionService(ConfigService config, DiscordSocketClient discordClient)
        {
            _config = config?.Config;
            _discordClient = discordClient;
        }
        public async Task InitializeAsync()
        {
            if (string.IsNullOrWhiteSpace(_config?.DiscordToken))
                throw new DiscordTokenNotProvidedOrNullException();

            await _discordClient.LoginAsync(TokenType.Bot, _config.DiscordToken);
            await _discordClient.StartAsync();
        }

        private class DiscordTokenNotProvidedOrNullException : Exception
        {
            public DiscordTokenNotProvidedOrNullException() : base("Discord token in config file is not provided or null.")
            {
            }
        }
    }
}