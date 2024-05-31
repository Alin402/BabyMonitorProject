using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class LivestreamCreationException : Exception
    {
        public LivestreamCreationException() { }

        public LivestreamCreationException(string message)
            : base(message) { }

        public LivestreamCreationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
