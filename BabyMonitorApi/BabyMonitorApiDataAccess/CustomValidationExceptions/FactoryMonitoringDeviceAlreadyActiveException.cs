using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class FactoryMonitoringDeviceAlreadyActiveException : Exception
    {
        public FactoryMonitoringDeviceAlreadyActiveException() { }

        public FactoryMonitoringDeviceAlreadyActiveException(string message)
            : base(message) { }

        public FactoryMonitoringDeviceAlreadyActiveException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
