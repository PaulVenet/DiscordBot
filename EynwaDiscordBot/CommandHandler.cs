using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;
using System;
using EynwaDiscordBot.Functions;
using System.Linq;
using Discord;
using System.Collections.Generic;
using EynwaDiscordBot.Models;
using Refit;
using Discord.Interop.Services;
using EynwaDiscordBot.Models.Constants;
using Eynwa.Interop.Services;

namespace EynwaDiscordBot
{
    class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private List<GameUpdateDate> userInGameList = new List<GameUpdateDate>();
        IUserService userService;
        IStatsService statsService;
        SocketGuild eynwaGuild;


        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();

            _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
            _client.UserJoined += _client_UserJoined;
            _client.ReactionAdded += _client_ReactionAdded;
            _client.ReactionRemoved += _client_ReactionRemoved;
            _client.GuildMemberUpdated += _client_GuildMemberUpdated;

            this.userService = RestService.For<IUserService>(SystemConstants.BaseUrl); // "http://91.121.178.28:5009/api");
            this.statsService = RestService.For<IStatsService>(SystemConstants.BaseUrl); // "http://91.121.178.28:5009/api");
        }

        private async Task _client_GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            if (arg1.Activity?.Name != arg2.Activity?.Name) // si un jeu est lancé ou arreter
            {
                if (arg1.Activity == null) // start game
                {
                    this.userInGameList.Add(new GameUpdateDate { Date = DateTime.Now, UserId = arg1.Id });
                    Console.WriteLine(arg1.Username + " à commencé a jouer à " + arg2.Activity);
                }
                else if (arg2.Activity == null) // end game
                {
                    //TODO send infos to API
                    var session = this.userInGameList.First(f => f.UserId == arg1.Id);
                    if (session != null)
                    {
                        string time = (DateTime.Now - session.Date).TotalMinutes.ToString();
                        this.userInGameList.Remove(session);
                        Console.WriteLine(arg1.Username + " à arreté de jouer à " + arg1.Activity + "duré de l'activité : " + time);
                        if (arg1.Activity.ToString() != "Spotify")
                        {
                            await statsService.Create(new Eynwa.Models.Entities.Stats.GameSessions
                            {
                                GameName = arg1.Activity.ToString(),
                                Timing = time,
                                UserId = arg1.Id.ToString()
                            });
                        }
                    }
                }
                else // change game
                {
                    var session = this.userInGameList.First(f => f.UserId == arg1.Id);
                    if (session != null)
                    {
                        string time = (DateTime.Now - session.Date).TotalMinutes.ToString();
                        this.userInGameList.Remove(session);
                        Console.WriteLine(arg1.Username + " est passer de " + arg1.Activity + " a " + arg2.Activity + "duré de l'activité : " + time);
                        if(arg1.Activity.ToString() != "Spotify")
                        {
                            await statsService.Create(new Eynwa.Models.Entities.Stats.GameSessions
                            {
                                GameName = arg1.Activity.ToString(),
                                Timing = time,
                                UserId = arg1.Id.ToString()
                            });
                        }
                    }
                    this.userInGameList.Add(new GameUpdateDate { Date = DateTime.Now, UserId = arg2.Id });
                }
            }
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasCharPrefix('!', ref argPos))
            {
                // REMPLISSAGE USER DB
                //var result = await _service.ExecuteAsync(context, argPos);
                //if (!result.IsSuccess)
                //{
                //    this.eynwaGuild = _client.GetGuild(248520271357542410);
                //    foreach (var user in this.eynwaGuild.Users)
                //    {
                //        if (!user.IsBot)
                //        {
                //            var role = user.Roles.ElementAt(1)?.Name;

                //            if (user.Roles.ElementAt(1)?.Name == "DJ")
                //            {
                //                role = user.Roles.ElementAt(2)?.Name;
                //            }
                //            await userService.Create(new Models.Entities.Account.UserInfo
                //            {
                //                DiscordId = user.Id.ToString(),
                //                Discriminator = user.Discriminator,
                //                Name = user.Username,
                //                Roles = role
                //            });
                //        }
                //    }
                //}

                //await context.Channel.SendMessageAsync("Fanfreluche !!!! cette commande n'exite pas.");
                await context.Message.DeleteAsync();
            }
        }

        [Command(RunMode = RunMode.Async)]
        private async Task _client_UserJoined(SocketGuildUser arg)
        {
            if (arg == null) return;
            await Roles.GetInstance().AddRole(Roles.Joueur, arg);

            //Ajout du nouvel l'utilisateur dans la base (API)
            if (!arg.IsBot)
            {
                try
                {
                    var usersList = await this.userService.GetAllUsers();
                    var results = usersList.Where(r => r.DiscordId == arg.Id.ToString());
                    if (results?.Count() != 0)
                    {
                        return;
                    }
                    else
                    {
                        await userService.Create(new Models.Entities.Account.UserInfo
                        {
                            DiscordId = arg.Id.ToString(),
                            Discriminator = arg.Discriminator,
                            Name = arg.Username,
                            Roles = Roles.Joueur
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        [Command(RunMode = RunMode.Async)]
        private async Task _client_ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            this.eynwaGuild = _client.GetGuild(248520271357542410);
            var user = this.eynwaGuild.GetUser(arg3.UserId);
            if (arg1.Id == 472563805306748938) //clique sur un icon de choix d'accès
            {
                string emotName = arg3.Emote.Name;
                switch (emotName)
                {
                    case "🎵":
                        await Roles.GetInstance().AddRole(Roles.Dj, user);
                        break;
                    case "xam":
                        var catTest = this.eynwaGuild.GetChannel(472563281802821643);
                        OverwritePermissions testPermit = new OverwritePermissions(readMessageHistory: PermValue.Allow,
                                                                                           readMessages: PermValue.Allow,
                                                                                           sendMessages: PermValue.Allow,
                                                                                           speak: PermValue.Allow,
                                                                                           useVoiceActivation: PermValue.Allow,
                                                                                           connect: PermValue.Allow);
                        await catTest.AddPermissionOverwriteAsync(user, testPermit).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }
            }
        }

        [Command(RunMode = RunMode.Async)]
        private async Task _client_ReactionRemoved(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            var eynwaGuild = this._client.GetGuild(248520271357542410);
            var user = eynwaGuild.GetUser(arg3.UserId);
            if (arg1.Id == 472563805306748938) //clique sur un icon de choix d'accès
            {
                string emotName = arg3.Emote.Name;
                switch (emotName)
                {
                    case "🎵":
                        await Roles.GetInstance().RemoveRole(Roles.Dj, user);
                        break;
                    case "xam":
                        var catTest = eynwaGuild.GetChannel(472563281802821643);
                        await catTest.RemovePermissionOverwriteAsync(user).ConfigureAwait(false);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
