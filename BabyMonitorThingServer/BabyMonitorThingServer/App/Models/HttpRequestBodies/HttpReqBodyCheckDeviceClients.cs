using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Models.HttpRequestBodies
{
    public class HttpReqBodyCheckDeviceClients
    {
        public string? UserId { get; set; }
        public string? DeviceId { get; set; }
    }
}
