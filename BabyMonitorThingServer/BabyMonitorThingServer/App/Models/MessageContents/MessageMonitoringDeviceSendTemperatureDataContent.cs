using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Models.MessageContents
{
    public class MessageMonitoringDeviceSendTemperatureDataContent
    {
        public string? UserID { get; set; }
        public double? TemperatureF { get; set; }
        public double? TemperatureC { get; set; }
        public double? Humidity { get; set; }
    }
}
