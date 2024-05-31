using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorFileServer
{
    public class RequestHandler
    {
        public async static Task<string> ReturnResponse(HttpListenerResponse res, string content, int statusCode)
        {
            try
            {
                res.StatusCode = statusCode;
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                await res.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                res.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return "";
        }
    }
}
