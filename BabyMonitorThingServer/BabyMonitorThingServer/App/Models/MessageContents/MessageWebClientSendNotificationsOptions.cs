namespace BabyMonitorThingServer.App.Models.MessageContents;

public class MessageWebClientSendNotificationsOptions
{
    public string? UserId { get; set; }
    public string? DeviceId { get; set; }
    public Dictionary<string, bool>? NotificationsOptions { get; set; }
}