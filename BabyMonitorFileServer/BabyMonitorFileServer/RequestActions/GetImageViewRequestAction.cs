using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorFileServer.RequestActions
{
    public class GetImageViewRequestAction : IHttpRequestAction
    {
        public async Task Execute(HttpListenerContext context)
        {
            var req = context.Request;
            var res = context.Response;

            try
            {
                if (req.Url != null && !string.IsNullOrEmpty(req.Url.AbsolutePath))
                {
                    string url = req.Url.AbsolutePath.ToLower();
                    int index = url.LastIndexOf('/');
                    string? imagePath = "";

                    if (index != -1)
                    {
                        imagePath = url.Substring(index + 1);

                        string? storageLocation = ConfigurationManager.AppSettings["StorageLocation"];
                        string fullImagePath = $"{storageLocation}\\{imagePath}";

                        if (!File.Exists(fullImagePath))
                        {
                            await RequestHandler.ReturnResponse(res, "Image not found", 404);
                        }

                        string htmlResponse = $"<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>{imagePath}</title>\r\n    <style>\r\n        .img {{\r\n            width: 40rem;\r\n            position: absolute;\r\n            left: 50%;\r\n            transform: translateX(-50%);\r\n        }}\r\n    </style>\r\n</head>\r\n<body style=\"background-color: black;\">\r\n    <img class=\"img\" src=\"http://localhost:6060/image/{imagePath}\" />\r\n</body>\r\n</html>";
                        res.ContentLength64 = htmlResponse.Length;
                        res.ContentType = "text/html";

                        await RequestHandler.ReturnResponse(res, htmlResponse, 200);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await RequestHandler.ReturnResponse(res, "Server Error", 500);
                throw new Exception(ex.Message);
            }
        }
    }
}
