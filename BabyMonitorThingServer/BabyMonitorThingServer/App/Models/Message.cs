using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.Types;

namespace BabyMonitorThingServer.App.Models
{
    public class Message
    {
        public ClientTypes? ClientType { get; set; }
        public MessageTypes? MessageType { get; set; }
        public Object? Content { get; set; }
    }
}
