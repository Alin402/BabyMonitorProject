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
    public class MessageActionMonitoringDeviceSendTemperatureDataValidator : IMessageValidator
    {
        public async Task<bool> Validate(Message message)
        {
			try
			{
                if (message.Content == null)
                {
                    return false;
                }

                var messageContent = JsonSerializer.Deserialize<MessageMonitoringDeviceSendTemperatureDataContent>((JsonElement)message.Content);
                if (messageContent.UserID == null || messageContent.TemperatureC == null ||
                    messageContent.TemperatureF == null || messageContent.Humidity == null)
                {
                    return false;
                }

                return true;
            }
			catch ( Exception ex)
			{
				Console.WriteLine(ex.Message);
                return false;
			}
        }
    }
}
