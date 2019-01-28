using Discord.Interop.Common;
using Newtonsoft.Json;
using Refit;
using Refit.Insane.PowerPack.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Interop.Logic
{
    public interface IServiceLogic
    {
        Task<Response<A>> MakeRequest<I, A>(Expression<Func<I, Task<A>>> expression,
                                                         Action<ApiException, Response<A>> apiExceptionAction = null,
                                                         Action<HttpRequestException, Response<A>> httpRequestExeptionAction = null,
                                                         Action<JsonSerializationException, Response> jsonExceptionAction = null,
                                                         Func<(int retryCount, Func<int, TimeSpan> delayProvider, Action<Exception, TimeSpan> exceptionHandler)> retryParamsProvider = null)
                    where I : IService
                    where A : class;

    }
}
