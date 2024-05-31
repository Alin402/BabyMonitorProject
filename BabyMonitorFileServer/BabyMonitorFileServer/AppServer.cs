using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HttpMultipartParser;

namespace BabyMonitorFileServer
{
    public class AppServer
    {
        private int _port;
        private bool _listening = false;
        private string _publicUri;
        private string _storageLocation;
        private HttpRequestsHandler _httpRequestsHandler;

        public AppServer(string port, string storageLocation)
        {
            _port = int.Parse(port);
            _publicUri = $"http://*:{port}/";
            _storageLocation = storageLocation;
            _httpRequestsHandler = new HttpRequestsHandler();
        }

        public async Task ListenAsync()
        {
            HttpListener listener = new();
            listener.Prefixes.Add(_publicUri);
            listener.Start();
            Console.WriteLine($"Listening on port {_port}");

            try
            {
                while (listener.IsListening)
                {
                    try
                    {
                        HttpListenerContext context = await listener.GetContextAsync();

                        await _httpRequestsHandler.HandleHttpRequest(context, _storageLocation);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Console.WriteLine("Stopped listening...");
                listener.Stop();
            }
        }
    }
}
