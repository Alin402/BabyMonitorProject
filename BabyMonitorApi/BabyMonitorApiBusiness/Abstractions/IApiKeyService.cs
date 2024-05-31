using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Entities;

namespace BabyMonitorApiBusiness.Abstractions
{
    public interface IApiKeyService
    {
        Task<bool> VerifyApiKey(ApiKey apiKey);
    }
}
