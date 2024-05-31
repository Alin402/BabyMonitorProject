using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.AI;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.MessageContents;
using BabyMonitorThingServer.App.Models.Notifications;
using BabyMonitorThingServer.App.Security.MessagesValidation;

namespace BabyMonitorThingServer.App.Messages.ConcreteMessageActions
{
    public class MessageActionNewMonitoringDeviceClientConnected : IMessageAction
    {
        public IMessageValidator Validator { get; set; }

        public MessageActionNewMonitoringDeviceClientConnected(IMessageValidator validator)
        {
            Validator = validator;
        }

        AppAiServer appAiServer = new AppAiServer();

        public async void Execute(string messageString, Message message, WebSocket webSocket)
        {
            if (await Validator.Validate(message))
            {
                try
                {
                    var messageContent = JsonSerializer.Deserialize<MessageNewMonitoringDeviceClientConnectedContent>((JsonElement)message.Content);
                    if (!AppData.Instance.OneToOneClientIDKeyValuePair.ContainsKey(messageContent.DeviceID))
                    {
                        AppData.Instance.OneToOneClientIDKeyValuePair[messageContent.DeviceID] = messageContent.UserID;
                        AppData.Instance.AddInClientIDToWebSocketKeyValuePair(messageContent.DeviceID, webSocket);
                    }

                    // notify clients that the monitoring device has connected
                    await AppMessages.NotifyUsersAboutMonitoringDeviceState(webSocket, new Notification()
                    {
                        Type = Types.NotificationTypes.MONITORING_DEVICE_ONLINE_NOTIFICATION,
                        Messages = new List<string>()
                        {
                            "The monitoring device is back online"
                        }
                    });
                    
                    await appAiServer.ReceiveAiDataAsync(messageContent.StreamingChannelUrl, messageContent.UserID, messageContent.DeviceID);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in message action new monitoring device client connected: {ex.Message}");
                }
            }
            else
            {
                throw new Exception("Message not valid");
            }
        }
    }
}
