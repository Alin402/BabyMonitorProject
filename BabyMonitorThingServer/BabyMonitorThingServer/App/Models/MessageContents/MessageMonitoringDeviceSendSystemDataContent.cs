using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Models.MessageContents
{
    public class MessageMonitoringDeviceSendSystemDataContent
    {
        public string UserID { get; set; }
        public double SystemTemperature { get; set; }
    }
}
