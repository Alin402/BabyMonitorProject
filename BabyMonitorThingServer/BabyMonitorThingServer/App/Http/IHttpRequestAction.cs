﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Http
{
    public interface IHttpRequestAction
    {
        Task Execute(HttpListenerContext context);
    }
}