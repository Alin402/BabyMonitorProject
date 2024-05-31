using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HttpMultipartParser;
using System.Configuration;
using System.Runtime.InteropServices;

namespace BabyMonitorFileServer.RequestActions
{
    public class UploadFileRequestAction : IHttpRequestAction
    {
        public async Task Execute(HttpListenerContext context)
        {
            var req = context.Request;
            var res = context.Response;

            try
            {
                var parser = MultipartFormDataParser.Parse(req.InputStream);
                var file = parser.Files[0];
                Stream stream = file.Data;

                string uploadedFileName = $"{GenerateRandomString(20)}_{file.FileName}";
                string? storageLocation = ConfigurationManager.AppSettings["StorageLocation"];
                using (FileStream fs = File.Create($"{storageLocation}\\{uploadedFileName}"))
                {
                    await stream.CopyToAsync(fs);
                }

                await RequestHandler.ReturnResponse(res, uploadedFileName, 200);
            }
            catch (Exception ex)
            {
                await RequestHandler.ReturnResponse(res, "Server Error", 500);
                throw new Exception(ex.Message);
            }
        }

        private string GenerateRandomString(int length)
        {
            StringBuilder stringBuilder = new StringBuilder(length);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }
            return stringBuilder.ToString();
        }
    }
}
