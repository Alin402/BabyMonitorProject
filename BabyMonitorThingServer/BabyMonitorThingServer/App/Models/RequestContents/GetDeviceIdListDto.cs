﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Models.RequestContents
{
    public class GetDeviceIdListDto
    {
        public List<Guid> MonitoringDeviceKeys { get; set; }
    }
}
