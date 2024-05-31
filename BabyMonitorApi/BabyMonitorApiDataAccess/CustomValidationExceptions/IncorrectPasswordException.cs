using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException() { }

        public IncorrectPasswordException(string message)
            : base(message) { }

        public IncorrectPasswordException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
