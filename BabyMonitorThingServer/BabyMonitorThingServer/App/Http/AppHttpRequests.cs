using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.Http.ConcreteHttpRequestsActions;

namespace BabyMonitorThingServer.App.Http
{
    public class AppHttpRequests
    {
        private readonly ConcurrentDictionary<string, IHttpRequestAction> _endPointToRequestActionKeyValuePair;

        public AppHttpRequests()
        {
            _endPointToRequestActionKeyValuePair = new ConcurrentDictionary<string, IHttpRequestAction>();
            _endPointToRequestActionKeyValuePair.TryAdd(Endpoints.CHECK_MONITORING_DEVICE_CLIENTS_CONNECTED, new HttpRequestActionCheckDeviceClients());
        }
        
        public async Task HandleHttpRequest(HttpListenerContext context)
        {
            try
            {
                if (context.Request.Url != null)
                {
                    var url = context.Request.Url.AbsolutePath.ToLower();
                    var action = _endPointToRequestActionKeyValuePair[url];
                    await action.Execute(context);
                }
                context.Response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in handle http request: {ex.Message}");
                await RequestHandler.ReturnResponse(context.Response, "Server Error", 500);
            }
            finally
            {
                context.Response.Close();
            }
        }
    }
}