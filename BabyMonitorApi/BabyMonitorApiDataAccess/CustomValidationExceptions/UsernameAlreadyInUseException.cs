using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class UsernameAlreadyInUseException : Exception
    {
        public UsernameAlreadyInUseException() { }

        public UsernameAlreadyInUseException(string message)
            : base(message) { }

        public UsernameAlreadyInUseException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
