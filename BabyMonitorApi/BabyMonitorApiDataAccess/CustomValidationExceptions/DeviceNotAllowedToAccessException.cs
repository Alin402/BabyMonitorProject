using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class DeviceNotAllowedToAccessException : Exception
    {
        public DeviceNotAllowedToAccessException() { }

        public DeviceNotAllowedToAccessException(string message)
            : base(message) { }

        public DeviceNotAllowedToAccessException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
