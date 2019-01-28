using Discord.Commands;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EynwaDiscordBot.Modules
{
    public class Random : ModuleBase<SocketCommandContext>
    {
        [Command("Random")]
        public async Task Rand(string input)
        {
            var v = this.Context.Channel.Name;
            input = Regex.Replace(input, @"\s+", "");
            if (Regex.IsMatch(input, @"^[a-zA-Z0-9,]+$") && !string.IsNullOrEmpty(input) && input.Contains(","))
            {
                string splitedItem = ",";
                string[] item = input.Split(char.Parse(splitedItem));
                System.Random rand = new System.Random();
                int value = rand.Next(0, item.Length);
                await Context.Channel.SendMessageAsync("Le Prophète Muhammad à dit : " + item[value]);
            }
            else
            {
                await Context.Channel.SendMessageAsync("la commande s'utilise comme ça (avec des virugles qui separe les choix) : !random choix1,choix2,choix3,choix4");
            }
        }
    }
}
