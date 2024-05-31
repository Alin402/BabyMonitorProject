using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorThingServer.App.Http
{
    public class RequestHandler
    {
        public async static Task<string> GetRequestBody(HttpListenerRequest req)
        {
            try
            {
                using (Stream body = req.InputStream)
                {
                    using (StreamReader reader = new StreamReader(body))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
			catch (Exception ex)
			{
                Console.WriteLine($"Exception in get request body: {ex.Message}");
			}
            return "";
        }

        public async static Task<string> ReturnResponse(HttpListenerResponse res, string content, int statusCode)
        {
            res.StatusCode = statusCode;
            res.ContentType = "text/html";
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(content);
                res.StatusCode = statusCode;
                await res.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
    }
}
