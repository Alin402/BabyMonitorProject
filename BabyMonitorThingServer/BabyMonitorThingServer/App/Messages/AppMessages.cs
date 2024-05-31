using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.AI;
using BabyMonitorThingServer.App.Messages.ConcreteMessageActions;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.Notifications;
using BabyMonitorThingServer.App.Security.MessagesValidation;
using BabyMonitorThingServer.App.Security.MessagesValidation.ConcreteMessageValidators;
using BabyMonitorThingServer.App.Types;

namespace BabyMonitorThingServer.App.Messages
{
    public class AppMessages
    {
        private AppServer appServer;

        public ConcurrentDictionary<MessageTypes, IMessageAction> MessageTypeActionValuePair { get; set; }

        public AppMessages(AppServer appServer)
        {
            this.appServer = appServer;

            // associate each type of message with a concrete action
            MessageTypeActionValuePair = new ConcurrentDictionary<MessageTypes, IMessageAction>();

            MessageTypeActionValuePair.TryAdd(MessageTypes.NEW_MONITORING_DEVICE_CLIENT_CONNECTION,
                new MessageActionNewMonitoringDeviceClientConnected(new MessageActionNewMonitoringDeviceClientConnectedValidator()));

            MessageTypeActionValuePair.TryAdd(MessageTypes.NEW_WEB_CLIENT_CONNECTED, 
                new MessageActionNewWebClientConnected(new MessageActionNewWebClientConnectedValidator()));

            MessageTypeActionValuePair.TryAdd(MessageTypes.MONITORING_DEVICE_SEND_TEMPERATURE_DATA,
                new MessageActionMonitoringDeviceSendTemperatureData(new MessageActionMonitoringDeviceSendTemperatureDataValidator()));

            MessageTypeActionValuePair.TryAdd(MessageTypes.MONITORING_DEVICE_SEND_SYSTEM_DATA,
                new MessageActionMonitoringDeviceSendSystemData(new MessageActionMonitoringDeviceSendSystemDataValidator()));

            MessageTypeActionValuePair.TryAdd(MessageTypes.WEB_CLIENT_SEND_NOTIFICATIONS_OPTIONS,
                new MessageWebClientSendNotificationsOptionsAction(
                    new MessageActionWebClientSendNotificationsOptionsValidator()));
        }

        public async Task ReceiveMessagesAsync(WebSocket webSocket)
        {
            try
            {
                while (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseSent)
                {
                    // get data from received message dynamically
                    using (MemoryStream messageStream = new MemoryStream())
                    {
                        // get the result from the socket, chunk by chunk
                        int INITIAL_MESSAGE_BUFFER_SIZE = int.Parse(ConfigurationManager.AppSettings["INITIAL_MESSAGE_BUFFER_SIZE"]);
                        byte[] messageDataBuffer = new byte[INITIAL_MESSAGE_BUFFER_SIZE];
                        WebSocketReceiveResult result;

                        do
                        {
                            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(messageDataBuffer), CancellationToken.None);
                            await messageStream.WriteAsync(messageDataBuffer, 0, result.Count);
                        }
                        while (!result.EndOfMessage);

                        // get the message string and convert it to object
                        string messageString = Encoding.UTF8.GetString(messageStream.ToArray());
                        Message? message = null;

                        if (!string.IsNullOrEmpty(messageString))
                        {
                            message = JsonSerializer.Deserialize<Message>(messageString);
                        }

                        if (message?.MessageType != null)
                        {
                            try
                            {
                                IMessageAction action = MessageTypeActionValuePair[(MessageTypes)message.MessageType];
                                action.Execute(messageString, message, webSocket);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Notification notification = new Notification()
                {
                    Type = NotificationTypes.MONITORING_DEVICE_OFFLINE_NOTIFICATION,
                    Messages = new List<string>()
                    {
                        "The monitoring device has disconnected"
                    }
                };
                await NotifyUsersAboutMonitoringDeviceState(webSocket, notification);

                AppData.Instance.RemoveFromClientIDToWebSocketKeyValuePair(webSocket);
                Console.WriteLine("Socket disconnected");
            }
        }

        public static async Task NotifyUsersAboutMonitoringDeviceState(WebSocket webSocket, Notification notification)
        {
            // get client id
            string clientID = AppData.Instance.WebSocketToClientIDKeyValuePair[webSocket];
            if (!string.IsNullOrEmpty(clientID))
            {
                // find if client is monitoring device
                if (AppData.Instance.OneToOneClientIDKeyValuePair.ContainsKey(clientID))
                {
                    // find user associated to the monitoring device
                    string userID = AppData.Instance.OneToOneClientIDKeyValuePair[clientID];
                    if (!string.IsNullOrEmpty(userID))
                    {
                        // get web sockets associated to user
                        if (AppData.Instance.ClientIDToWebSocketsKeyValuePair.ContainsKey(userID))
                        {
                            List<WebSocket> userWebSockets = AppData.Instance.ClientIDToWebSocketsKeyValuePair[userID];

                            string notificationString = JsonSerializer.Serialize(notification);
                            await MessageSender.BroadcastMessage(notificationString, userWebSockets);
                        }
                    }
                }
            }
        }
    }
}