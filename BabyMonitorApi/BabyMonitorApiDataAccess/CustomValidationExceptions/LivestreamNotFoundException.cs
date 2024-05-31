using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class LivestreamNotFoundException : Exception
    {
        public LivestreamNotFoundException() { }

        public LivestreamNotFoundException(string message)
            : base(message) { }

        public LivestreamNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
