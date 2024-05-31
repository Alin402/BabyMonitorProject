using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.AI;
using BabyMonitorThingServer.App.Http;
using BabyMonitorThingServer.App.Messages;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.Notifications;

namespace BabyMonitorThingServer.App
{
    public class AppServer
    {
        // App server state
        private int port;
        private bool listening = false;
        private string publicURI;
        private AppMessages appMessages;
        private AppHttpRequests appHttpRequests;

        public AppServer(string port)
        {
            // Initialize state
            this.port = int.Parse(port);
            appMessages = new AppMessages(this);
            appHttpRequests = new AppHttpRequests();

            // Get public uri prefix from configuration file and initialize the public uri
            string? publicUriPrefix = ConfigurationManager.AppSettings["PUBLIC_URI_PREFIX"];
            publicURI = $"{publicUriPrefix}:{port}/";
        }

        public async Task ListenToWebSocketsAndHttpRequestsAsync()
        {
            // Listener that would listen to upcoming http messages
            HttpListener httpListener = new();
            try
            {
                // Add the public uri as a prefix for the http listener and start listening
                httpListener.Prefixes.Add(publicURI);
                httpListener.Start();
                listening = true;
                Console.WriteLine($"Listening on port {port}");
                // Listen for upcoming connections
                while (listening)
                {
                    try
                    {
                        HttpListenerContext context = await httpListener.GetContextAsync();

                        if (!context.Request.IsWebSocketRequest)
                        {
                            // Handle cors
                            // context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                            // context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                            // context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                            // if (context.Request.HttpMethod == "OPTIONS")
                            // {
                            //     context.Response.StatusCode = (int)HttpStatusCode.OK;
                            //     context.Response.Close();
                            // }
                            
                            Console.WriteLine("New http request");
                            _ = Task.Run(async () => await appHttpRequests.HandleHttpRequest(context));
                        }

                        _ = Task.Run(async () => await HandleNewWebSocketConnectionAsync(context));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception handling
                Console.WriteLine(ex.Message);
                listening = false;
            }
            finally
            {
                // Finally stop the httpListener
                listening = false;
                httpListener.Stop();
            }
        }

        private async Task HandleNewWebSocketConnectionAsync(HttpListenerContext context)
        {
            try
            {
                // Get the web socket context from the http listener
                HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                // Get the web socket from the context
                WebSocket webSocket = webSocketContext.WebSocket;
                Console.WriteLine("New socket connected");

                // For each new web socket connection, listen for messages
                _ = Task.Run(async () => await appMessages.ReceiveMessagesAsync(webSocket));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}