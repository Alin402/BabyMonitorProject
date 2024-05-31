using System.Net.WebSockets;
using System.Text.Json;
using BabyMonitorThingServer.App.Livestream;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.MessageContents;
using BabyMonitorThingServer.App.Security.MessagesValidation;

namespace BabyMonitorThingServer.App.Messages.ConcreteMessageActions;

public class MessageWebClientSendNotificationsOptionsAction : IMessageAction
{
    public IMessageValidator Validator { get; set; }

    public MessageWebClientSendNotificationsOptionsAction(IMessageValidator validator)
    {
        Validator = validator;
    }

    public async void Execute(string messageString, Message message, WebSocket webSocket)
    {
        try
        {
            if (!await Validator.Validate(message))
            {
                throw new Exception("Invalid message in message web client send notification options action");
            }

            var messageContent = JsonSerializer.Deserialize<MessageWebClientSendNotificationsOptions>((JsonElement)message.Content);
            if (messageContent != null)
            {
                var userId = messageContent.UserId;
                var deviceId = messageContent.DeviceId;
                var notificationsOptions = messageContent.NotificationsOptions;

                if (LivestreamData.Instance.DeviceIdLivestreamDataKeyValuePair.ContainsKey(deviceId))
                {
                    var saveLivestream = LivestreamData.Instance.DeviceIdLivestreamDataKeyValuePair[deviceId];
                    saveLivestream.NotificationsOptions = notificationsOptions;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception in message web client send notification options action {e.Message}");
        }
    }
}