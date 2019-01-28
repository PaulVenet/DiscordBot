using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EynwaDiscordBot.Models.Diagnostics
{
    public class HttpDiagnosticsHandler : DelegatingHandler
    {
        public HttpDiagnosticsHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        public HttpDiagnosticsHandler() : base(new HttpClientHandler())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponseMessage;
            HttpResponseMessage response;

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(10000); // ms

            using (new DebugProcessTimer("Total ellapsed time"))
            {
                Debug.WriteLine($"Request: {request}");
                if (request?.Content != null)
                {
                    string str = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(str))
                    {
                        str = JToken.Parse(str).ToString();
                        Debug.WriteLine($"\n\nRequest Content: {JToken.Parse(str)}");
                    }
                }

                using (new DebugProcessTimer("Response time"))
                {
                    response = await base.SendAsync(request, new CancellationToken());
                    Debug.WriteLine($"Response: {response}");
                    if (response?.Content != null)
                    {
                        string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        if (!string.IsNullOrEmpty(str))
                        {
                            str = JToken.Parse(str).ToString();
                            Debug.WriteLine($"\n\nResponse Content: {str}");
                        }
                    }
                }
            }
            httpResponseMessage = response;
            return httpResponseMessage;
        }
    }
}
