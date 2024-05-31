using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorFileServer.Endpoints.ConcreteEndpoints
{
    public class StandardEndpoint : IEndpoint
    {
        private string _name;
        public IHttpRequestAction Action { get; set; }

        public StandardEndpoint(string name, IHttpRequestAction action)
        {
            _name = name;
            Action = action;
        }

        public bool IsValidEndpoint(string url)
        {
            return url == _name;
        }
    }
}
