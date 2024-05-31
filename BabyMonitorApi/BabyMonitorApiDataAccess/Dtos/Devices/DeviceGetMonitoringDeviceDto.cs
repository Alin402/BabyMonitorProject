using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Dtos.Devices
{
    public class DeviceGetMonitoringDeviceDto
    {
        public Guid ApiKeyId { get; set; }
        public string ApiKeyValue { get; set; }
        public Guid DeviceId { get; set; }
    }
}
