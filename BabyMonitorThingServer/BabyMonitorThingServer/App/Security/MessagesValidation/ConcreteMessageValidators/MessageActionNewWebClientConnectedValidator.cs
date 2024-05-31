using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.MessageContents;
using Newtonsoft.Json;

namespace BabyMonitorThingServer.App.Security.MessagesValidation.ConcreteMessageValidators
{
    public class MessageActionNewWebClientConnectedValidator : IMessageValidator
    {
        private const string url = "http://68.219.120.90:5000/api/users/authenticate/server";
        public async Task<bool> Validate(Message message)
        {
            try
            {
                if (message.Content == null)
                {
                    throw new Exception("Message content not valid");
                }

                var messageContent = System.Text.Json.JsonSerializer.Deserialize<MessageNewWebClientConnectedContent>((JsonElement)message.Content);

                // Check if user credentials are valid
                string? email = messageContent.Email;
                string? password = messageContent.Password;

                // Make request to verify api key
                using (var client = new HttpClient())
                {
                    var body = new
                    {
                        Email = email,
                        Password = password
                    };

                    var jsonString = JsonConvert.SerializeObject(body);
                    var postContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, postContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return false;
        }
    }
}
