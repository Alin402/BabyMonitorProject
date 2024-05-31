﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.Messages;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.HttpRequestBodies;
using BabyMonitorThingServer.App.Security;
using BabyMonitorThingServer.App.Types;

namespace BabyMonitorThingServer.App.Http.ConcreteHttpRequestsActions
{
    public class HttpRequestActionCheckDeviceClients : IHttpRequestAction
    {
        public async Task Execute(HttpListenerContext context)
        {
            var req = context.Request;
            var res = context.Response;
            try
            {
                var bodyString = await RequestHandler.GetRequestBody(req);
                var body = JsonSerializer.Deserialize<HttpReqBodyCheckDeviceClients>(bodyString);

                var userId = body?.UserId?.ToLower();
                var deviceId = body?.DeviceId?.ToLower();
                
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(deviceId))
                {
                    await RequestHandler.ReturnResponse(res, "Bad Request", 400);
                }
                
                if (!AppData.Instance.OneToManyClientIDKeyValuePair.ContainsKey(userId ?? ""))
                {
                    await RequestHandler.ReturnResponse(res, "Client Id not found", 404);
                }

                if (!AppData.Instance.OneToManyClientIDKeyValuePair[userId ?? ""].Contains(deviceId ?? ""))
                {
                    await RequestHandler.ReturnResponse(res, "Forbidden", 403);
                }
                
                if (!AppData.Instance.OneToOneClientIDKeyValuePair.ContainsKey(deviceId ?? ""))
                {
                    await RequestHandler.ReturnResponse(res, "inactive", 200);
                }
                else
                {
                    await RequestHandler.ReturnResponse(res, "active", 200);
                }
            }
            catch (Exception ex)
            {
                await RequestHandler.ReturnResponse(context.Response, "Server Error", 500);
                context.Response.Close();
                throw new Exception(ex.Message);
            }
        }
    }
}