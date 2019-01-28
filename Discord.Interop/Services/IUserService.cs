using Discord.Interop.Common;
using EynwaDiscordBot.Models.Constants;
using EynwaDiscordBot.Models.Diagnostics;
using EynwaDiscordBot.Models.Entities.Account;
using Refit;
using Refit.Insane.PowerPack.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Interop.Services
{
    [ApiDefinition(SystemConstants.BaseUrl, typeof(HttpDiagnosticsHandler))]
    //TODO : change token to a variable with bearer behind
    //[Headers("Authorization:" + SystemConstants.SwaggerToken, "api_key:" + SystemConstants.ApiKey)]
    public interface IUserService : IService
    {
        [Post("/User")]
        Task<UserInfo> Create([Body] UserInfo param);

        [Get("/User/{uid}")]
        Task<UserInfo> GetUser(string uid);

        [Patch("/User/{uid}")]
        Task<UserInfo> PatchUser(string uid, [Body] UserInfo param);
    }
}
