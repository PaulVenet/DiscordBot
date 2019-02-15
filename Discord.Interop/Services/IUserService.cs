﻿using EynwaDiscordBot.Models.Constants;
using EynwaDiscordBot.Models.Entities.Account;
using Refit;
using Refit.Insane.PowerPack.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit.Insane.PowerPack.Data;

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

        [Patch("/User/{uid}")]
        Task<UserInfo> PatchUser(string uid, [Body] UserInfo param);

        [Delete("/User/{uid}")]
        Task<Response> DeleteUser(string uid);
    }
}
