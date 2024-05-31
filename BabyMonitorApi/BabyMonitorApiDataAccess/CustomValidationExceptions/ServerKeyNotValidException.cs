using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class ServerKeyNotValidException : Exception
    {
        public ServerKeyNotValidException() { }

        public ServerKeyNotValidException(string message)
            : base(message) { }

        public ServerKeyNotValidException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
