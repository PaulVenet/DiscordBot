using EynwaDiscordBot.Models.Constants;
using EynwaDiscordBot.Models.Entities.Account;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Interop.Services
{
    //TODO : change token to a variable with bearer behind
    [Headers("Content-Type:" + "application/json")]
    public interface IUserService
    {
        [Post("/User")]
        Task<UserInfo> Create([Body] UserInfo param);

        [Get("/User/{uid}")]
        Task<UserInfo> GetUser(string uid);

        [Get("/User")]
        Task<List<UserInfo>> GetAllUsers();

        [Put("/User/{uid}")]
        Task<UserInfo> PutUser(long uid, [Body] UserInfo param);
    }
}
