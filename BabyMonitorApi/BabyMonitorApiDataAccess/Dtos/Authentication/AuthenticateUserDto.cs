using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Dtos.Authentication
{
    public class AuthenticateUserDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        [MinLength(8)]
        public string? Password { get; set; }
    }
}
