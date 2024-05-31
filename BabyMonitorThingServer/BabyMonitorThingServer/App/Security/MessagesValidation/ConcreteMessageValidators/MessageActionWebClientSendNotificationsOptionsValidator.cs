using System.Text.Json;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.MessageContents;

namespace BabyMonitorThingServer.App.Security.MessagesValidation.ConcreteMessageValidators;

public class MessageActionWebClientSendNotificationsOptionsValidator : IMessageValidator
{
    public Task<bool> Validate(Message message)
    {
        try
        {
            if (message.Content == null)
            {
                throw new Exception("Null content in message action web client send notifications options validator");
            }

            var messageContent = JsonSerializer.Deserialize<MessageWebClientSendNotificationsOptions>(message?.Content?.ToString()!);
            if (string.IsNullOrEmpty(messageContent?.UserId) || messageContent.NotificationsOptions == null || messageContent.DeviceId == null)
            {
                return Task.FromResult(false);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception in message action web client send notifications options validator {e.Message}");
        }
        return Task.FromResult(true);
    }
}