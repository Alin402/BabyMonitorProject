using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Types
{
    public enum MessageTypes
    {
        NEW_MONITORING_DEVICE_CLIENT_CONNECTION,
        NEW_WEB_CLIENT_CONNECTED,
        NOTIFY_WEB_CLIENT_OF_MONITORING_DEVICE_STATE,
        NOTIFY_MONITORING_DEVICE_CLIENT_OF_WEB_CLIENT_STATE,
        MONITORING_DEVICE_SEND_TEMPERATURE_DATA,
        MONITORING_DEVICE_SEND_SYSTEM_DATA,
        NOTIFICATION,
        FACE_RECOGNITION_DETAILS,
        WEB_CLIENT_SEND_NOTIFICATIONS_OPTIONS
    }
}
