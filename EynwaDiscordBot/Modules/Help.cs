using Discord.Commands;
using System.Threading.Tasks;

namespace EynwaDiscordBot.Modules
{
    public class Test : ModuleBase<SocketCommandContext>
    {
        [Command("Help")]
        public async Task HelpUser()
        {
            await Context.Channel.SendMessageAsync("List des commandes disponibles :!random , !topgame, !rank");
        }
    }
}
