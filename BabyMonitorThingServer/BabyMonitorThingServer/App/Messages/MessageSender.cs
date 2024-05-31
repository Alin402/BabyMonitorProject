using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Messages
{
    public class MessageSender
    {
        private static readonly object webSocketLock = new object();
        public static async Task SendMessage(string message, WebSocket webSocket)
        {
            try
            {
                bool endOfMessage = true;
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                lock (webSocketLock)
                {
                    if (webSocket != null && message != null)
                    {
                        webSocket.SendAsync(messageBuffer, WebSocketMessageType.Text, endOfMessage, CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in send message: {ex.Message}");
            }
        }

        public static async Task BroadcastMessage(string message, List<WebSocket> webSockets)
        {
            try
            {
                if (webSockets.Count < 1)
                {
                    throw new Exception("No clients to send the message to");
                }

                foreach (var ws in webSockets)
                {
                    await SendMessage(message, ws);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in broadcast message: {ex.Message}");
            }
        }
    }
}
