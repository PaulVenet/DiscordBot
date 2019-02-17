﻿using Discord.Commands;
using Eynwa.Interop.Services;
using Eynwa.Models.Entities.Stats;
using EynwaDiscordBot.Models.Constants;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EynwaDiscordBot.Modules
{
    public class SessionsStats : ModuleBase<SocketCommandContext>
    {
        IStatsService statsService;

        public SessionsStats()
        {
            this.statsService = RestService.For<IStatsService>(SystemConstants.BaseUrl); // "http://91.121.178.28:5009/api");
        }

        [Command("Rank", RunMode = RunMode.Async)]
        public async Task rank()
        {
            var user = Context.Message.Author;
            var startDate = DateTime.Now.Add(new TimeSpan(-7,0,0,0));
            var sessions = await this.statsService.GetAllSessions(dateStart : startDate.ToString(), dateEnd : DateTime.Now.ToString()); //get sessions from the last 7 days
            
            List<GameSessions> unifyUserList = new List<GameSessions>();

            var sessionsOfUser = sessions.Where(s => s.UserId == user.Id.ToString());
            var totalMinutesOfWeekForUser = sessionsOfUser.Sum(s => (long)Convert.ToDouble(s.Timing));
            foreach (var session in sessions)
            {
                if(unifyUserList.Any(i => i.UserId == session.UserId.ToString()))
                {
                    //SI unifyUserList contient déjà l'utilisateur
                    foreach (var sessionUnify in unifyUserList)
                    {
                        if (sessionUnify.UserId == session.UserId)
                        {
                            sessionUnify.Timing = ((long)Convert.ToDouble(sessionUnify.Timing) + (long)Convert.ToDouble(session.Timing)).ToString();
                        }
                    }
                }
                else
                {
                    unifyUserList.Add(session);
                }
            }

            var rankingList = unifyUserList.OrderByDescending(t => t.Timing).ToList();
            var totalUser = rankingList.Count;
            int position = rankingList.FindIndex(a => a.UserId == user.Id.ToString()) + 1;
            if(position < 1)
            {
                await Context.Channel.SendMessageAsync($"Tu n'es pas classé, laisse ta vie de coter pour ça !");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"Tu es classé(e) {position} sur {totalUser} avec un total de {totalMinutesOfWeekForUser} minutes de jeu.");
            }
        }

        [Command("TopGame", RunMode = RunMode.Async)]
        public async Task topGame()
        {
            var startDate = DateTime.Now.Add(new TimeSpan(-7, 0, 0, 0));
            var sessions = await this.statsService.GetAllSessions(dateStart: startDate.ToString(), dateEnd: DateTime.Now.ToString()); //get sessions from the last 7 days

            List<GameSessions> unifyGameList = new List<GameSessions>();
            foreach (var session in sessions)
            {
                if (unifyGameList.Any(i => i.GameName == session.GameName))
                {
                    //SI unifyUserList contient déjà l'utilisateur
                    foreach (var sessionUnify in unifyGameList)
                    {
                        if (sessionUnify.UserId == session.UserId)
                        {
                            sessionUnify.Timing = ((long)Convert.ToDouble(sessionUnify.Timing) + (long)Convert.ToDouble(session.Timing)).ToString();
                        }
                    }
                }
                else
                {
                    unifyGameList.Add(session);
                }
            }

            var rankingList = unifyGameList.OrderByDescending(t => t.Timing).ToList();
            var totalGame = rankingList.Count;
            await Context.Channel.SendMessageAsync($"Top Game de la semaine : \n\n" +
                $":first_place: {rankingList[0]?.GameName} avec {rankingList[0]?.Timing} minutes. \n" +
                $":second_place: {rankingList[1]?.GameName} avec {rankingList[1]?.Timing} minutes. \n" +
                $":third_place: {rankingList[2]?.GameName} avec {rankingList[2]?.Timing} minutes. \n" +
                $":four: {rankingList[3]?.GameName} avec {rankingList[3]?.Timing} minutes.\n" +
                $":five: {rankingList[4]?.GameName} avec {rankingList[4]?.Timing} minutes.\n" );
        }

        //[Command("TopUser", RunMode = RunMode.Async)]
        //public async Task topUser()
        //{
        //    var message = await Context.Channel.SendMessageAsync($"Top");
        //}
    }
}
