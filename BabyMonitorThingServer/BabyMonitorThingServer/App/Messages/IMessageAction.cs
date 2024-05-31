using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.Models;
using BabyMonitorThingServer.App.Security.MessagesValidation;

namespace BabyMonitorThingServer.App.Messages
{
    public interface IMessageAction
    {
        public IMessageValidator Validator { get; set; }
        void Execute(string messageString, Message message, WebSocket webSocket);
    }
}
