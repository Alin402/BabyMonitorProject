using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorThingServer.App.Models;

namespace BabyMonitorThingServer.App.Security.MessagesValidation
{
    public interface IMessageValidator
    {
        Task<bool> Validate(Message message);
    }
}
