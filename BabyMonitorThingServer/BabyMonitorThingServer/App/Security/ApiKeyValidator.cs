using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BabyMonitorThingServer.App.Security
{
    public class ApiKeyValidator
    {
        private static string url = "http://68.219.120.90:5000/api/keys/verify";
        // this is temporary for testing purposes
        static List<string> keys = new List<string>
        {
            "huebhdjfbhbfd",
            "sdfsdfbsjdhbf"
        };
        public static async Task<bool> ValidateApiKey(Guid? apiKeyId, string apiKeyValue)
        {
            try
            {
                // Make request to verify api key
                using (var client = new HttpClient())
                {
                    var body = new
                    {
                        Id = apiKeyId,
                        Value = apiKeyValue
                    };

                    var jsonString = JsonConvert.SerializeObject(body);
                    var postContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, postContent);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in api key validator: {ex.Message}");
            }
            return false;
        }
    }
}
