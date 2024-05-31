using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Models.MessageContents
{
    public class MessageNewWebClientConnectedContent
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserID { get; set; }
    }
}
