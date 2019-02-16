using Eynwa.Models.Entities.Stats;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eynwa.Interop.Services
{
    //TODO : change token to a variable with bearer behind
    [Headers("Content-Type:" + "application/json")]
    public interface IStatsService
    {
        [Post("/Sessions")]
        Task<GameSessions> Create([Body] GameSessions param);

        [Get("/Sessions/{uid}")]
        Task<GameSessions> GetSession(string uid);

        [Get("/Sessions")]
        Task<List<GameSessions>> GetAllSessions();

        [Patch("/Sessions/{uid}")]
        Task<GameSessions> PatchSessions(string uid, [Body] GameSessions param);
    }
}
