using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EynwaDiscordBot.Modules
{
    public class Mhw : ModuleBase<SocketCommandContext>
    {
        [Command("Mhw")]
        public async Task mhw()
        {
            var message = await Context.Channel.SendMessageAsync($"Monster Hunter World Est dispo !! :rathalos:");
        }
    }
}
