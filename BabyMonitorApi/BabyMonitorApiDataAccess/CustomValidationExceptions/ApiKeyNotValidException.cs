using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class ApiKeyNotValidException : Exception
    {
        public ApiKeyNotValidException() { }

        public ApiKeyNotValidException(string message)
            : base(message) { }

        public ApiKeyNotValidException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
