using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System.Diagnostics;
using Emgu.CV;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Timers;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.MessageContents;
using System.Text.Json;
using BabyMonitorThingServer.App.Messages;
using System.Net.WebSockets;
using System.Configuration;
using BabyMonitorThingServer.App.Livestream;
using BabyMonitorThingServer.App.Models.Livestream;
using BabyMonitorThingServer.App.Models.Notifications;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MemoryStream = System.IO.MemoryStream;

namespace BabyMonitorThingServer.App.AI
{
    public class AppAiServer
    {
        private AmazonRekognitionClient _rekognitionClient;

        // These are in miliseconds
        private const int CHECK_IF_RECEIVE_FRAMES_DELAY = 1000;
        private const int EXTRACT_FRAME_INTERVAL = 500;
        public AppAiServer()
        {
            // Get amazon rekognition credentials and configure the api client
            string? accessKey = ConfigurationManager.AppSettings["AMAZON_ACCESS_KEY"];
            string? secretKey = ConfigurationManager.AppSettings["AMAZON_SECRET_KEY"];
            var region = RegionEndpoint.USEast1;
            var credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);

            _rekognitionClient = new AmazonRekognitionClient(credentials, region);
        }

        public async Task ReceiveAiDataAsync(string streamChannelUrl, string userId, string deviceId)
        {
            var webSockets = new List<WebSocket>();
            // Run while the device is connected
            while (AppData.Instance.OneToOneClientIDKeyValuePair.ContainsKey(deviceId))
            {
                try
                {
                    // Try to capture frames from the live stream
                    var capture = new VideoCapture(streamChannelUrl);

                    // If no frames were captures, wait one second and try again
                    if (!capture.IsOpened)
                    {
                        await Task.Delay(CHECK_IF_RECEIVE_FRAMES_DELAY);
                        continue;
                    }

                    // Once frames were received, start processing them
                    Console.WriteLine("Extracting AI data...");

                    // Notify connected clients that the livestream has started
                    var deviceOnlineNotification = new Notification
                    {
                        MessageType = Types.MessageTypes.NOTIFICATION,
                        Type = Types.NotificationTypes.LIVESTREAM_STARTED_NOTIFICATION,
                        Messages = ["Livestream started"]
                    };

                    webSockets = AppData.Instance.GetWebSocketsAssociatedWithClient(userId);

                    await NotifyUserAboutLivestreamState(webSockets, deviceOnlineNotification);
                    // Start capturing livestream data
                    LivestreamData.Instance.StartRecordingLivestreamData(deviceId);
                    if (FirebaseApp.DefaultInstance == null)
                    {
                        FirebaseApp.Create(new AppOptions()
                        {
                            Credential = GoogleCredential.FromFile("C:\\Users\\balan\\Desktop\\dada\\babymonitornotifications-9c998-firebase-adminsdk-rqjcu-072203c790.json")
                        });   
                    }
                    CaptureFrames(capture, webSockets, deviceId);
                    capture.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in receive ai data {ex.Message}");
                    await Task.Delay(CHECK_IF_RECEIVE_FRAMES_DELAY);
                    continue;
                }
            }

            // Notify users that the device has been disconnected and the livestream has stopped
            Console.WriteLine("Device disconnected. Stopped extracting AI data...");
            // Save livestream data
            var saveLivestream = LivestreamData.Instance.DeviceIdLivestreamDataKeyValuePair[deviceId];
            if (saveLivestream != null)
            {
                await saveLivestream.SaveLivestream();
            }
            Notification deviceOfflineNotification = new Notification
            {
                MessageType = Types.MessageTypes.NOTIFICATION,
                Type = Types.NotificationTypes.LIVESTREAM_ENDED_NOTIFICATION,
                Messages = new List<string>() { "Livestream ended" }
            };

            if (webSockets != null && webSockets.Count > 0)
            {
                await NotifyUserAboutLivestreamState(webSockets, deviceOfflineNotification);
            }
        }

        private void CaptureFrames(VideoCapture capture, List<WebSocket> userClientsWebSockets, string deviceId)
        {
            Bitmap? currentFrame = null;
            // Using a timer so that a frame is selected to be fed to the AI 
            // After a certain time interval
            using (System.Timers.Timer faceDetailsTimer = new(EXTRACT_FRAME_INTERVAL))
            {
                faceDetailsTimer.Elapsed += (sender, e) => FaceDetailsTimer_Elapsed(sender, e, userClientsWebSockets, currentFrame, deviceId);
                faceDetailsTimer.Start();
                while (true)
                {
                    try
                    {
                        // Getting the frame and converting it to bitmap
                        Mat frame = new Mat();
                        if (!capture.Read(frame))
                        {
                            break;
                        }

                        currentFrame = BitmapExtension.ToBitmap(frame);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception in capture frames: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        private void FaceDetailsTimer_Elapsed(object? sender, ElapsedEventArgs e, List<WebSocket> userClientsWebSockets, Bitmap currentFrame, string deviceId)
        {
            // if a frame was found, feed it to the AI api
            if (currentFrame != null)
            {
                GetFaceDetails(currentFrame, userClientsWebSockets, deviceId);
            }
        }

        private async void GetFaceDetails(Bitmap image, List<WebSocket> userClientsWebSockets, string deviceId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Save the image to the memory stream and insert the bytes to an amazon type image
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                var image1 = new Amazon.Rekognition.Model.Image
                {
                    Bytes = ms
                };

                // Request body
                var detectFacesRequest = new DetectFacesRequest()
                {
                    Image = image1,
                    Attributes = new List<string>() { "ALL" },
                };

                try
                {
                    var detectFacesResponse = await _rekognitionClient.DetectFacesAsync(detectFacesRequest);
                    var face = detectFacesResponse.FaceDetails[0];
                    
                    var saveLivestream = LivestreamData.Instance.DeviceIdLivestreamDataKeyValuePair[deviceId];
                    var emotion = face.Emotions.Select(e => e.Type.ToString()).ToList().First();
                    var babyAwake = face.EyesOpen.Value;
                    if (saveLivestream != null)
                    {
                        saveLivestream.CurrentBabyState = new BabyState()
                        {
                            Emotion = emotion,
                            Awake = babyAwake
                        };
                    }
                    
                    var message = new Message
                    {
                        MessageType = Types.MessageTypes.FACE_RECOGNITION_DETAILS,
                        ClientType = Types.ClientTypes.ADMIN,
                        Content = new MessageSendFaceRecognitionDetailsContent
                        {
                            Emotions = face.Emotions.Select(e => e.Type.ToString()).ToList(),
                            BoundingBox = [face.BoundingBox.Left, face.BoundingBox.Top, face.BoundingBox.Width, face.BoundingBox.Height],
                            Awake = babyAwake
                        }
                    };
                    
                    var messageString = JsonSerializer.Serialize(message);
                    
                    await MessageSender.BroadcastMessage(messageString, userClientsWebSockets);

                    // string[] emotions = ["HAPPY", "SAD", "ANGRY", "CONFUSED", "DISGUSTED"];
                    // var index = new Random().Next(0, emotions.Length + 1);
                    // bool[] awake = [true, false];
                    // var awakeIndex = new Random().Next(0, 2);
                    //
                    // var emotion = emotions[index];
                    // var babyAwake = awake[awakeIndex];
                    // // Save baby state
                    // var saveLivestream = LivestreamData.Instance.DeviceIdLivestreamDataKeyValuePair[deviceId];
                    // if (saveLivestream != null)
                    // {
                    //     saveLivestream.CurrentBabyState = new BabyState()
                    //     {
                    //         Emotion = emotion,
                    //         Awake = babyAwake
                    //     };
                    // }
                    //
                    // Message message = new Message
                    // {
                    //     MessageType = Types.MessageTypes.FACE_RECOGNITION_DETAILS,
                    //     ClientType = Types.ClientTypes.ADMIN,
                    //     Content = new MessageSendFaceRecognitionDetailsContent
                    //     {
                    //         Emotions = [
                    //             emotion
                    //         ],
                    //         BoundingBox = [0, 0, 0, 0],
                    //         Awake = babyAwake
                    //     }
                    // };
                    //
                    // string messageString = JsonSerializer.Serialize(message);
                    //
                    // await MessageSender.BroadcastMessage(messageString, userClientsWebSockets);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception in get face details: {ex.Message}");
                }
            }
        }

        private async Task NotifyUserAboutLivestreamState(List<WebSocket> webSockets, Notification notification)
        {
            string notificationString = JsonSerializer.Serialize(notification);

            await MessageSender.BroadcastMessage(notificationString, webSockets);
        }
    }
}
