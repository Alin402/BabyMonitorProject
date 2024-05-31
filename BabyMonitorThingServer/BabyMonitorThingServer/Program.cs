using System.Net;
using System.Net.WebSockets;
using BabyMonitorThingServer;
using BabyMonitorThingServer.App;
using System.Configuration;

// Get the port from the configuration file
string? port = ConfigurationManager.AppSettings["PUBLIC_PORT"];

if (port != null)
{
    // Initialize new app server
    AppServer app = new AppServer(port);

    // Listen for messages
    await app.ListenToWebSocketsAndHttpRequestsAsync();
}