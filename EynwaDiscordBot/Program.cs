using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Audio;
using Refit.Insane.PowerPack.Services;
using Microsoft.Extensions.DependencyInjection;
using Discord.Interop.Services;
using Refit;

namespace EynwaDiscordBot
{
    class Program
    {
        static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandHandler _handler;
        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();
            await _client.SetGameAsync("Marabouter Freyja");
            await _client.LoginAsync(TokenType.Bot, "MzU4MzQ4MzQwMDAzMzQwMjg5.DJ3KCQ.JeokXGmI19EKq0WZNCitSgltfs8");
            await _client.StartAsync();
            _handler = new CommandHandler(_client);
            await Task.Delay(-1);
        }
    }
}
