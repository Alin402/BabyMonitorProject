using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App
{
    public class AppData
    {
        // Singleton pattern
        private AppData() { }
        private static AppData? instance = null;
        private static readonly object lockObject = new object();

        public static AppData Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new AppData();
                        }
                    }
                }
                return instance;
            }
        }

        // Dictionaries to define client relationships

        // One user account can be associated to multiple monitoring devices
        public ConcurrentDictionary<string, List<string>> OneToManyClientIDKeyValuePair { get; set; } = new ConcurrentDictionary<string, List<string>>();
        // One monitoring device can only be associated to one user account
        public ConcurrentDictionary<string, string> OneToOneClientIDKeyValuePair { get; set; } = new ConcurrentDictionary<string, string>();
        // One client can be associated to multiple web sockets
        public ConcurrentDictionary<string, List<WebSocket>> ClientIDToWebSocketsKeyValuePair { get; set; } = new ConcurrentDictionary<string, List<WebSocket>>();
        // One web socket can only be associated to one client
        public ConcurrentDictionary<WebSocket, string> WebSocketToClientIDKeyValuePair { get; set; } = new ConcurrentDictionary<WebSocket, string>();
        // One monitoring device is associated to one streaming channel
        public ConcurrentDictionary<string, string> OneToOneClientIdUrlKeyValuePair { get; set; } = new ConcurrentDictionary<string, string>();

        public List<WebSocket> GetWebSocketsAssociatedWithClient(string clientID)
        {
            try
            {
                return ClientIDToWebSocketsKeyValuePair[clientID];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in get web socket associated with client: {ex.Message}");
            }
            return new List<WebSocket>();
        }

        public void AddInClientIDToWebSocketKeyValuePair(string clientID, WebSocket webSocket)
        {
            try
            {
                if (ClientIDToWebSocketsKeyValuePair.ContainsKey(clientID))
                {
                    if (ClientIDToWebSocketsKeyValuePair[clientID].Contains(webSocket))
                    {
                        return;
                    }

                    ClientIDToWebSocketsKeyValuePair[clientID].Add(webSocket);
                }
                else
                {
                    ClientIDToWebSocketsKeyValuePair[clientID] = new List<WebSocket> { webSocket };
                }

                if (!WebSocketToClientIDKeyValuePair.ContainsKey(webSocket))
                {
                    WebSocketToClientIDKeyValuePair[webSocket] = clientID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in add in client id to web socket key value pair: {ex.Message}");
            }
        }

        public void RemoveFromClientIDToWebSocketKeyValuePair(WebSocket webSocket)
        {
            try
            {
                if (WebSocketToClientIDKeyValuePair.ContainsKey(webSocket))
                {
                    string clientID = WebSocketToClientIDKeyValuePair[webSocket];

                    ClientIDToWebSocketsKeyValuePair[clientID].Remove(webSocket);
                    if (ClientIDToWebSocketsKeyValuePair[clientID].Count == 0)
                    {
                        if (OneToManyClientIDKeyValuePair.ContainsKey(clientID))
                        {
                            OneToManyClientIDKeyValuePair.TryRemove(clientID, out _);
                        }
                        else
                        {
                            OneToOneClientIDKeyValuePair.TryRemove(clientID, out _);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in remove from client to id web socket key value pai: {ex.Message}");
            }
        }
    }
}
