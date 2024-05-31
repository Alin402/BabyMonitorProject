using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class FactoryMonitoringDeviceNotFoundException : Exception
    {
        public FactoryMonitoringDeviceNotFoundException() { }

        public FactoryMonitoringDeviceNotFoundException(string message)
            : base(message) { }

        public FactoryMonitoringDeviceNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
