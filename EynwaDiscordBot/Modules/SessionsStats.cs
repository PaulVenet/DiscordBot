using Discord.Commands;
using Eynwa.Interop.Services;
using Eynwa.Models.Entities.Stats;
using EynwaDiscordBot.Models.Constants;
using Refit;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            try
            {
                var sessionsOfUser = sessions.Where(s => s.UserId == user.Id.ToString()).ToList();
                foreach (var session in sessions)
                {
                    session.Timing = session.Timing.Replace(",", ".");
                    session.Timing = session.Timing.Split(".")[0];
                    if (unifyUserList.Any(i => i.UserId == session.UserId.ToString()))
                    {
                        //SI unifyUserList contient déjà l'utilisateur
                        foreach (var sessionUnify in unifyUserList)
                        {
                            if (sessionUnify.UserId == session.UserId)
                            {
                                sessionUnify.Timing = (int.Parse(sessionUnify.Timing, NumberStyles.Number) + int.Parse(session.Timing, NumberStyles.Number)).ToString();
                            }
                        }
                    }
                    else
                    {
                        unifyUserList.Add(session);
                    }
                }
                var totalMinutesOfWeekForUser = sessionsOfUser.Sum(s => int.Parse(s.Timing, NumberStyles.Number));

                var rankingList = unifyUserList.OrderByDescending(t => t.Timing).ToList();
                var totalUser = rankingList.Count;
                int position = rankingList.FindIndex(a => a.UserId == user.Id.ToString()) + 1;
                if (position < 1)
                {
                    await Context.Channel.SendMessageAsync($"Tu n'es pas classé, laisse ta vie de coter pour ça !");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"Tu es classé(e) {position} sur {totalUser} avec un total de {MinutesToHoursConverter(totalMinutesOfWeekForUser)}.");
                }
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
                Console.Write(e.InnerException);
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
                session.Timing = session.Timing.Replace(",", ".");
                session.Timing = session.Timing.Split(".")[0];
                if (unifyGameList.Any(i => i.GameName == session.GameName))
                {
                    //SI unifyUserList contient déjà l'utilisateur
                    foreach (var sessionUnify in unifyGameList)
                    {
                        if (sessionUnify.GameName == session.GameName)
                        {
                            sessionUnify.Timing = (int.Parse(sessionUnify.Timing, NumberStyles.Number) + int.Parse(session.Timing, NumberStyles.Number)).ToString();
                        }
                    }
                }
                else
                {
                    var temp = int.Parse(session.Timing, NumberStyles.Number);
                    session.Timing = temp.ToString();
                    unifyGameList.Add(session);
                }
            }

            var rankingList = unifyGameList.OrderByDescending(t => t.Timing).ToList();
            var totalGame = rankingList.Count;
            if(totalGame < 5)
            {
                await Context.Channel.SendMessageAsync("Il faut un minimum de 5 jeux jouer pour avoir un classement !");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"Top Game de la semaine : \n\n" +
                $":first_place: {rankingList[0]?.GameName} avec {MinutesToHoursConverter(int.Parse(rankingList[0]?.Timing, NumberStyles.Number))}. \n" +
                $":second_place: {rankingList[1]?.GameName} avec {MinutesToHoursConverter(int.Parse(rankingList[1]?.Timing, NumberStyles.Number))}. \n" +
                $":third_place: {rankingList[2]?.GameName} avec {MinutesToHoursConverter(int.Parse(rankingList[2]?.Timing, NumberStyles.Number))}. \n" +
                $":four: {rankingList[3]?.GameName} avec {MinutesToHoursConverter(int.Parse(rankingList[3]?.Timing, NumberStyles.Number))}.\n" +
                $":five: {rankingList[4]?.GameName} avec {MinutesToHoursConverter(int.Parse(rankingList[4]?.Timing, NumberStyles.Number))}.\n\n" +
                $"Un total de {totalGame} jeu ont été lancés ces 7 derniers jours");
            }
        }

        private string MinutesToHoursConverter(double minutes)
        {
            if(minutes > 59)
            {

                int hour = int.Parse(minutes.ToString(), NumberStyles.Number) / 60;
                int minutesRecalculated = int.Parse(minutes.ToString(), NumberStyles.Number) % 60;
                if(hour > 1)
                {
                    return $"{hour} heures et {minutesRecalculated} minutes";
                }
                else
                {
                    return $"{hour} heure et {minutesRecalculated} minutes";
                }
            }
            else
            {
                return $"{minutes} minutes";
            }
        }
    }
}
