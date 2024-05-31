using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class MonitoringDeviceNotFoundException : Exception
    {
        public MonitoringDeviceNotFoundException() { }

        public MonitoringDeviceNotFoundException(string message)
            : base(message) { }

        public MonitoringDeviceNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
