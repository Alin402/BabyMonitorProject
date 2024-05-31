using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorFileServer.Endpoints
{
    public interface IEndpoint
    {
        bool IsValidEndpoint(string url);
        public IHttpRequestAction Action { get; set; }
    }
}
