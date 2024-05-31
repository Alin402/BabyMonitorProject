using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class BabyStateCreationException : Exception
    {
        public BabyStateCreationException() { }

        public BabyStateCreationException(string message)
            : base(message) { }

        public BabyStateCreationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
