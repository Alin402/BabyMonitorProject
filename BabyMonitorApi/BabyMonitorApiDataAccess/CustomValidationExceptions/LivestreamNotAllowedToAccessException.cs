using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class LivestreamNotAllowedToAccessException : Exception
    {
        public LivestreamNotAllowedToAccessException() { }

        public LivestreamNotAllowedToAccessException(string message)
            : base(message) { }

        public LivestreamNotAllowedToAccessException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
