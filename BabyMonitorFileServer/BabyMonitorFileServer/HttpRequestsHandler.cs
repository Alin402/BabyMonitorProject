using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorFileServer.RequestActions;
using HttpMultipartParser;
using BabyMonitorFileServer.Endpoints;
using BabyMonitorFileServer.Endpoints.ConcreteEndpoints;

namespace BabyMonitorFileServer
{
    public class HttpRequestsHandler
    {
        private List<IEndpoint> _endpoints = new List<IEndpoint>();

        public HttpRequestsHandler()
        {
            _endpoints = new List<IEndpoint>();

            _endpoints.Add(new StandardEndpoint(EndpointNames.UPLOAD_FILE, new UploadFileRequestAction()));
            _endpoints.Add(new ImageUrlEndpoint(EndpointNames.GET_IMAGE, new GetFileRequestAction()));
            _endpoints.Add(new ImageViewUrlEndpoint(EndpointNames.SHOW_IMAGE, new GetImageViewRequestAction()));
        }

        public async Task HandleHttpRequest(HttpListenerContext context, string storageLocation)
        {
            if (context.Request.Url != null)
            {
                string url = context.Request.Url.AbsolutePath.ToLower();
                bool foundValid = false;

                foreach (var endpoint in _endpoints)
                {
                    if (endpoint.IsValidEndpoint(url))
                    {
                        await endpoint.Action.Execute(context);
                        foundValid = true;
                        break;
                    }
                }
                if (!foundValid)
                {
                    await RequestHandler.ReturnResponse(context.Response, "Wrong path", 404);
                }
            }
            context.Response.Close();
        }
    }
}
