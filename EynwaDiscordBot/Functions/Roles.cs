using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EynwaDiscordBot.Functions
{
    class Roles : ModuleBase<SocketCommandContext>
    {
        private static Roles instance = null;

        public static string Dj = "DJ";
        public static string Admin = "Lucanes(Admin)";
        public static string Modo = "Goliathus (Modo)";
        public static string Amis = "Dynastinae (amis)";
        public static string Joueur = "Oryctes (joueur)";
        public static string Bousier = "Bousiers";

        private Roles()
        {

        }

        public static Roles GetInstance()
        {
            return instance ?? (instance = new Roles());
        }

        public async Task AddRole(string roleName, IGuildUser user)
        {
            var role = user.Guild.Roles?.FirstOrDefault(x => x.Name == roleName);
            try
            {
                await user.AddRoleAsync(role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task RemoveRole(string roleName, IGuildUser user)
        {
            var role = user.Guild.Roles?.FirstOrDefault(x => x.Name == roleName);
            try
            {
                await user.RemoveRoleAsync(role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public bool? CheckIfAddSpecificRole(SocketGuildUser guildUser, SocketUser user, string roleName)
        {
            try
            {
                var role = guildUser.Guild.Roles.FirstOrDefault(x => x.Name == roleName);
                if (role != null)
                {
                    if (guildUser.Roles.Contains(role))
                    {
                        return true;
                    }
                    return false;
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
