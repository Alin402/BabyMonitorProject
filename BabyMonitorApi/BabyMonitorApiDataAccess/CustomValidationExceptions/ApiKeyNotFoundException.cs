using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class ApiKeyNotFoundException: Exception
    {
        public ApiKeyNotFoundException() { }

        public ApiKeyNotFoundException(string message)
            : base(message) { }

        public ApiKeyNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
