using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Text;
using System.Text.Json;
using BabyMonitorThingServer.App.Messages;
using BabyMonitorThingServer.App.Models.Livestream;
using BabyMonitorThingServer.App.Types;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Notification = BabyMonitorThingServer.App.Models.Notifications.Notification;

namespace BabyMonitorThingServer.App.Livestream;

public class SaveLivestreamData
{
    public Guid DeviceId { get; set; }
    public double? Time { get; set; } = 0;
    public string? DateStarted { get; set; }
    public string? ServerKey { get; set; }
    public List<BabyState>? BabyStates { get; set; } = new List<BabyState>();
    public BabyState? CurrentBabyState { get; set; }
    public System.Timers.Timer? Timer { get; set; }
    public System.Timers.Timer? Timer2 { get; set; }
    public Dictionary<string, bool> NotificationsOptions { get; set; }
    string registrationToken = "eiK1ym4XSy-y2tUG_i13fo:APA91bGnalxJrC3a7EwGwWgQNQbdvJu8qbHd4OQHBNw60aFs1rU1Dl_7eiOsJSBFvQizhK_MxZJJsf-BMWl_DW0PYKO_4op0GpHJC09-ycO5iAbTvilzYn2zDknTFaOsx8Megezfc7QA";

    private async void SendNotification(string title, string body)
    {
        var message = new Message()
        {
            Notification = new FirebaseAdmin.Messaging.Notification
            {
                Title = title,
                Body = body
            },
            Token = registrationToken,
            Android = new AndroidConfig()
            {
                Priority = Priority.High
            }
        };
        var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        Console.WriteLine("Successfully sent message: " + response);
    }

    private void HandleNotifications()
    {
        if (CurrentBabyState != null)
        {
            var emotion = CurrentBabyState.Emotion;
            switch (emotion)
            {
                case "HAPPY":
                    if (NotificationsOptions.ContainsKey("HAPPY"))
                    {
                        if (NotificationsOptions["HAPPY"])
                        {
                            SendNotification("Baby State:", "Your baby is happy!!!!");
                        }   
                    }
                    break;
                case "SAD":
                    if (NotificationsOptions.ContainsKey("SAD"))
                    {
                        if (NotificationsOptions["SAD"])
                        {
                            SendNotification("Baby State:", "Your baby is sad :(");
                        }   
                    }
                    break;
                case "CALM":
                    if (NotificationsOptions.ContainsKey("CALM"))
                    {
                        if (NotificationsOptions["CALM"])
                        {
                            SendNotification("Baby State:", "Your baby is calm");
                        }   
                    }
                    break;
                case "DISGUSTED":
                    if (NotificationsOptions.ContainsKey("DISGUSTED"))
                    {
                        if (NotificationsOptions["DISGUSTED"])
                        {
                            SendNotification("Baby State:", "Your baby is disgusted");
                        }   
                    }
                    break;
                case "CONFUSED":
                    if (NotificationsOptions.ContainsKey("CONFUSED"))
                    {
                        if (NotificationsOptions["CONFUSED"])
                        {
                            SendNotification("Baby State:", "Your baby is confused");
                        }   
                    }
                    break;
                case "SURPRISED":
                    if (NotificationsOptions.ContainsKey("SURPRISED"))
                    {
                        if (NotificationsOptions["SURPRISED"])
                        {
                            SendNotification("Baby State:", "Your baby is surprised");
                        }   
                    }
                    break;
                case "ANGRY":
                    if (NotificationsOptions.ContainsKey("ANGRY"))
                    {
                        if (NotificationsOptions["ANGRY"])
                        {
                            SendNotification("Baby State:", "Your baby is angry");
                        }   
                    }
                    break;
                case "FEAR":
                    if (NotificationsOptions.ContainsKey("FEAR"))
                    {
                        if (NotificationsOptions["FEAR"])
                        {
                            SendNotification("Baby State:", "Your baby is fearful");
                        }   
                    }
                    break;
            }
        }
    }

    private void HandleSaveBabyStates()
    {
        if (CurrentBabyState != null)
        {
            Time++;
            CurrentBabyState.AtSecond = Time;
            Console.WriteLine($"{CurrentBabyState.AtSecond}, {CurrentBabyState.Emotion}, {CurrentBabyState.Awake}");
            BabyStates?.Add(CurrentBabyState);
        }
    }
    
    public void OnElapsedInterval(object source, EventArgs e)
    {
        HandleSaveBabyStates();
    }
    
    public void OnElapsedInterval2(object source, EventArgs e)
    {
        HandleNotifications();
    }

    public async Task SaveLivestream()
    {
        Timer?.Stop();
        Timer2?.Stop();
        var apiUrl = ConfigurationManager.AppSettings["API_ADDRESS"];
        try
        {
            var body = new
            {
                DeviceId = DeviceId,
                Time = Time,
                DateStarted = DateStarted,
                ServerKey = ServerKey,
                BabyStates = BabyStates
            };
            var stringBody = JsonSerializer.Serialize(body);
            using (var client = new HttpClient())
            {
                var content = new StringContent(stringBody, Encoding.UTF8, "application/json");
                var res = await client.PostAsync($"{apiUrl}/livestream/create", content);

                var notification = new Notification();
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Successfully saved livestream data");
                    notification.Type = NotificationTypes.SAVED_LIVESTREAM_DETAILS_NOTIFICATION;
                    notification.Messages = ["Successfully saved livestream data"];
                }
                else
                {
                    Console.WriteLine("Problem when saving livestream data");
                    notification.Type = NotificationTypes.UNABLE_TO_SAVE_LIVESTREAM_DETAILS_NOTIFICATION;
                    notification.Messages = ["Problem when trying to save livestream data"];
                }
                Console.WriteLine(AppData.Instance.OneToOneClientIDKeyValuePair.Keys.Count);
                // Send acknowledgement notification
                var userId = AppData.Instance.OneToOneClientIDKeyValuePair[DeviceId.ToString()];
                if (!string.IsNullOrEmpty(userId))
                {
                    // Get websockets
                    var sockets = AppData.Instance.GetWebSocketsAssociatedWithClient(userId);
                    if (sockets.Count > 0)
                    {
                        var notificationString = JsonSerializer.Serialize(notification);
                        await MessageSender.BroadcastMessage(notificationString, sockets);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in save livestream {ex.Message}");
        }
    }
}