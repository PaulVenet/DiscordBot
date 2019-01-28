using Discord.Interop.Logic;
using Discord.Interop.Services;
using EynwaDiscordBot.Models.Entities.Account;
using Refit.Insane.PowerPack.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Logic.User
{
    public class UserLogic : IUserLogic
    {
        private readonly IServiceLogic serviceLogic;

        public string UserToken { get; set; }
        public (double latitude, double longitude)? UserPosition { get; set; }
        public bool HasUserPosition => this.UserPosition.HasValue;

        public UserLogic(IServiceLogic serviceLogic)
        {
            this.serviceLogic = serviceLogic;
        }

        public async Task<Response<UserInfo>> CreateAccount(long discrodId, string name, string discriminator, EynwaDiscordBot.Models.Enum.Roles role)
        {
            UserInfo param = new UserInfo
            {
                DiscordId = discrodId,
                Name = name,
                Discriminator = discriminator,
                Roles = role
            };

            var response = await this.serviceLogic.MakeRequest<IUserService, UserInfo>(api => api.Create(param));

            return response;
        }

        public async Task<Response<UserInfo>> GetUserById(string uid)
        {
            var result = await this.serviceLogic.MakeRequest<IUserService, UserInfo>
            (api => api.GetUser(uid));

            return result;
        }

        public async Task<Response<UserInfo>> PatchUser(string uid, UserInfo userInfo)
        {
            var result = await this.serviceLogic.MakeRequest<IUserService, UserInfo>
            (api => api.PatchUser(uid, userInfo));

            return result;
        }
    }
}
