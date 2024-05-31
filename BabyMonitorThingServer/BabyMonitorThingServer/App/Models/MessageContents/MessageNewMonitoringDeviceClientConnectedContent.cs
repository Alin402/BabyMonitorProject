using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Models.MessageContents
{
    public class MessageNewMonitoringDeviceClientConnectedContent
    {
        public Guid? ApiKeyId { get; set; }
        public string? ApiKeyValue { get; set; }
        public string? DeviceID { get; set; }
        public string? UserID { get; set; }
        public string? StreamingChannelUrl { get; set; }
    }
}
