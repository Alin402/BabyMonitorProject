using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Dtos.Devices
{
    public class GetDeviceIdListDto
    {
        public List<Guid> MonitoringDeviceKeys { get; set; }
    }
}
