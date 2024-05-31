using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BabyMonitorFileServer.Endpoints.ConcreteEndpoints
{
    public class ImageViewUrlEndpoint : IEndpoint
    {
        private string _name;
        private string? _url;
        public IHttpRequestAction Action { get; set; }

        public ImageViewUrlEndpoint(string name, IHttpRequestAction action)
        {
            _name = name;
            Action = action;
        }

        public bool IsValidEndpoint(string url)
        {
            Console.WriteLine(url);
            string pattern = @$"/view/([^?]*)";

            Regex regex = new Regex(pattern);
            if (regex.IsMatch(url))
            {
                _url = url;
                return true;
            }
            return false;
        }
    }
}
