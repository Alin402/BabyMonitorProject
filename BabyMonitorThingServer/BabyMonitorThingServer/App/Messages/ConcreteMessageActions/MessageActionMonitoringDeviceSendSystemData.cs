using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.MessageContents;
using BabyMonitorThingServer.App.Security.MessagesValidation;

namespace BabyMonitorThingServer.App.Messages.ConcreteMessageActions
{
    public class MessageActionMonitoringDeviceSendSystemData : IMessageAction
    {
        public IMessageValidator Validator { get; set; }

        public MessageActionMonitoringDeviceSendSystemData(IMessageValidator validator)
        {
            Validator = validator;
        }

        public async void Execute(string messageString, Message message, WebSocket webSocket)
        {
            try
            {
                if (!await Validator.Validate(message))
                {
                    return;
                }

                var messageContent = JsonSerializer.Deserialize<MessageMonitoringDeviceSendSystemDataContent>((JsonElement)message.Content);

                List<WebSocket> webSockets = AppData.Instance.GetWebSocketsAssociatedWithClient(messageContent.UserID);
                if (webSockets.Count < 1)
                {
                    return;
                }

                await MessageSender.BroadcastMessage(messageString, webSockets);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
