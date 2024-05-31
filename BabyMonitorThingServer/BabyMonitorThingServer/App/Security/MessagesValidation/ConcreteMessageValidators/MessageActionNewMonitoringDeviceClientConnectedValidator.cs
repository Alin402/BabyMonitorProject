using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Models.MessageContents;

namespace BabyMonitorThingServer.App.Security.MessagesValidation.ConcreteMessageValidators
{
    public class MessageActionNewMonitoringDeviceClientConnectedValidator : IMessageValidator
    {
        public async Task<bool> Validate(Message message)
        {
            try
            {
                if (message.Content == null)
                {
                    throw new Exception("Message content not valid");
                }

                var messageContent = JsonSerializer.Deserialize<MessageNewMonitoringDeviceClientConnectedContent>((JsonElement)message.Content);

                bool apiKeyValid = await ApiKeyValidator.ValidateApiKey(messageContent.ApiKeyId, messageContent.ApiKeyValue);

                if (!apiKeyValid)
                {
                    throw new Exception("Api key not valid");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
