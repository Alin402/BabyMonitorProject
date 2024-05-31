using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class BabyNotFoundException : Exception
    {
        public BabyNotFoundException() { }

        public BabyNotFoundException(string message)
            : base(message) { }

        public BabyNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
