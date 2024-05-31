using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorFileServer.RequestActions
{
    public class GetFileRequestAction : IHttpRequestAction
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

                        byte[] fileBytes = File.ReadAllBytes(fullImagePath);
                        if (fileBytes.Length == 0) 
                        {
                            await RequestHandler.ReturnResponse(res, "File not found", 404);
                            return;
                        }

                        res.ContentType = "image/*";
                        res.ContentLength64 = fileBytes.Length;
                        res.AddHeader("Content-Disposition", "inline");

                        using (Stream outputStream = res.OutputStream)
                        {
                            await outputStream.WriteAsync(fileBytes);
                            res.Close();
                        }
                    }

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        Console.WriteLine(imagePath);
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
