using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class MonitoringDeviceAlreadyExistsException : Exception
    {
        public MonitoringDeviceAlreadyExistsException() { }

        public MonitoringDeviceAlreadyExistsException(string message)
            : base(message) { }

        public MonitoringDeviceAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
