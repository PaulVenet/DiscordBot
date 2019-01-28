using EynwaDiscordBot.Models.Entities.Account;
using Refit.Insane.PowerPack.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Interop.Logic
{
    public interface IUserLogic
    {
        bool HasUserPosition { get; }
        (double latitude, double longitude)? UserPosition { get; set; }
        string UserToken { get; set; }

        Task<Response<UserInfo>> CreateAccount(long discordId, string name, string discriminator, EynwaDiscordBot.Models.Enum.Roles role);
        Task<Response<UserInfo>> GetUserById(string uid);
        Task<Response<UserInfo>> PatchUser(string uid, UserInfo userInfo);
    }
}
