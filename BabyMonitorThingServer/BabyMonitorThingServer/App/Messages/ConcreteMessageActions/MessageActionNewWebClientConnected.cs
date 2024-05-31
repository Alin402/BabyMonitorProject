using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.MessageContents;
using BabyMonitorThingServer.App.Models.RequestContents;
using BabyMonitorThingServer.App.Security.MessagesValidation;
using Newtonsoft.Json;

namespace BabyMonitorThingServer.App.Messages.ConcreteMessageActions
{
    public class MessageActionNewWebClientConnected : IMessageAction
    {
        public IMessageValidator Validator { get; set; }
        private const string url = "http://68.219.120.90:5000/api/device/get/ids";

        public MessageActionNewWebClientConnected(IMessageValidator validator)
        {
            Validator = validator;
        }

        public async void Execute(string messageString, Message message, WebSocket webSocket)
        {
            try
            {
                if (await Validator.Validate(message))
                {
                    Console.WriteLine("New message from web client trying to connect...");
                    var messageContent = System.Text.Json.JsonSerializer.Deserialize<MessageNewWebClientConnectedContent>((JsonElement)message.Content);

                    using (var client = new HttpClient())
                    {
                        string requestUrl = $"{url}/{messageContent?.UserID}?serverKey=85159608637075381844";
                        HttpResponseMessage response = await client.GetAsync(requestUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string jsonContent = await response.Content.ReadAsStringAsync();
                            GetDeviceIdListDto? deviceIdsContent = JsonConvert.DeserializeObject<GetDeviceIdListDto>(jsonContent);

                            if (deviceIdsContent != null)
                            {
                                List<string> deviceIds = deviceIdsContent.
                                    MonitoringDeviceKeys.
                                    ToList().
                                    Select(id => id.ToString()).
                                    ToList();

                                string? userId = messageContent?.UserID.ToString().ToLower();

                                if (!AppData.Instance.OneToManyClientIDKeyValuePair.ContainsKey(userId))
                                {
                                    AppData.Instance.OneToManyClientIDKeyValuePair.TryAdd(userId, deviceIds);
                                }
                                AppData.Instance.AddInClientIDToWebSocketKeyValuePair(userId, webSocket);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    throw new Exception("Message not valid");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in message action new web client connected: {ex.Message}");
            }
        }
    }
}
