using Discord.Interop.Common;
using Discord.Interop.Logic;
using Newtonsoft.Json;
using Polly;
using Refit;
using Refit.Insane.PowerPack.Data;
using Refit.Insane.PowerPack.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Eynwa.Logic.Services
{
    public class ServiceLogic : IServiceLogic
    {
#if DEBUG
        private const int RetryCount = 0;
#else
            private const int RetryCount = 1;
#endif
        // In seconds
        private const int RetryDelay = 2;
        private IRestService restService;

        public ServiceLogic(IRestService restService)
        {
            this.restService = restService;
        }

        public async Task<Response<A>> MakeRequest<I, A>(Expression<Func<I, Task<A>>> expression,
                                                 Action<ApiException, Response<A>> apiExceptionAction = null,
                                                 Action<HttpRequestException, Response<A>> httpRequestExeptionAction = null,
                                                 Action<JsonSerializationException, Response> jsonExceptionAction = null,
                                                 Func<(int retryCount, Func<int, TimeSpan> delayProvider, Action<Exception, TimeSpan> exceptionHandler)> retryParamsProvider = null)
            where I : IService
            where A : class
        {
            var response = new Response<A>();

            try
            {
                if (retryParamsProvider != null)
                {
                    var retryParams = retryParamsProvider.Invoke();

                    response = await Policy.Handle<Exception>()
                                           .WaitAndRetryAsync(
                                                           retryParams.retryCount,
                                                           retryParams.delayProvider,
                                                           retryParams.exceptionHandler)
                                           .ExecuteAsync(() =>
                                           {
                                               return this.restService.Execute<I, A>(expression);
                                           });
                }
                else
                {
                    response = await this.restService.Execute<I, A>(expression);
                }
            }
            catch (ApiException apiException)
            {
                Debug.WriteLine(apiException);

                response.AddErrorMessage(apiException.Message);
                apiExceptionAction?.Invoke(apiException, response);
            }
            catch (HttpRequestException httpRequestException)
            {
                Debug.WriteLine(httpRequestException);

                response.AddErrorMessage(httpRequestException.Message);
                httpRequestExeptionAction?.Invoke(httpRequestException, response);
            }
            catch (JsonSerializationException jsonException)
            {
                Debug.WriteLine(jsonException);

                response.AddErrorMessage(jsonException.Message);
                jsonExceptionAction?.Invoke(jsonException, response);
            }
            catch (Exception otherException)
            {
                Debug.WriteLine(otherException);
                response.AddErrorMessage(otherException.Message);
            }

            return response;
        }
    }
}
