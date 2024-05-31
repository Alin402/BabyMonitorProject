using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.Types;

namespace BabyMonitorThingServer.App.Models.Notifications
{
    public class Notification
    {
        public MessageTypes? MessageType { get; set; } = MessageTypes.NOTIFICATION;
        public NotificationTypes Type { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
