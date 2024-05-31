using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class BabyNotAllowedToAccessException : Exception
    {
        public BabyNotAllowedToAccessException() { }

        public BabyNotAllowedToAccessException(string message)
            : base(message) { }

        public BabyNotAllowedToAccessException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
