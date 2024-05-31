using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class NotEnoughLivestreamInformationException : Exception
    {
        public NotEnoughLivestreamInformationException() { }

        public NotEnoughLivestreamInformationException(string message)
            : base(message) { }

        public NotEnoughLivestreamInformationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
