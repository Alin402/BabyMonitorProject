using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.CustomValidationExceptions
{
    public class UploadBabyPictureException : Exception
    {
        public UploadBabyPictureException() { }

        public UploadBabyPictureException(string message)
            : base(message) { }

        public UploadBabyPictureException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
