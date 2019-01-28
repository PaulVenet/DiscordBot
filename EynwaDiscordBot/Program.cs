using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord.Audio;
using Discord.Interop.Logic;
using Discord.Logic.User;
using Eynwa.Logic.Services;
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
        //private IUserLogic userLogic;
        public async Task StartAsync()
        {
            var restServiceBuilde = new RestServiceBuilder()
                .WithCaching()
                .WithAutoRetry();

            //var test5 = typeof(Program).GetElementType();
            //var oui = test5.Assembly;

            //var testazre = restServiceBuilde.BuildRestService(oui);

            var userService = RestService.For<IUserService>("https://api.iextrading.com");

            // setup our DI
            var serviceProvider = new ServiceCollection()
            .AddSingleton<IUserLogic, UserLogic>()
            .BuildServiceProvider();

            //configure console logging
            var userlogic = serviceProvider
                .GetService<IUserLogic>();


            _client = new DiscordSocketClient();
            await _client.SetGameAsync("Marabouter Freyja");
            await _client.LoginAsync(TokenType.Bot, "MzU4MzQ4MzQwMDAzMzQwMjg5.DJ3KCQ.JeokXGmI19EKq0WZNCitSgltfs8");
            await _client.StartAsync();
            _handler = new CommandHandler(_client, userlogic);
            await Task.Delay(-1);
        }
    }
}
