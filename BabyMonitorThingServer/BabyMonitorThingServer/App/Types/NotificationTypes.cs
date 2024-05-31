using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Types
{
    public enum NotificationTypes
    {
        MONITORING_DEVICE_ONLINE_NOTIFICATION,
        MONITORING_DEVICE_OFFLINE_NOTIFICATION,
        LIVESTREAM_STARTED_NOTIFICATION,
        LIVESTREAM_ENDED_NOTIFICATION,
        SAVED_LIVESTREAM_DETAILS_NOTIFICATION,
        UNABLE_TO_SAVE_LIVESTREAM_DETAILS_NOTIFICATION
    }
}
