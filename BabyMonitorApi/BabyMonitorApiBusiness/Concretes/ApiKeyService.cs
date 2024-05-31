using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Entities;
using BabyMonitorApiDataAccess.Repositories.Abstractions;

namespace BabyMonitorApiBusiness.Concretes
{
    public class ApiKeyService : IApiKeyService
    {
		IRepository<ApiKey> _apiKeyRepository;

        public ApiKeyService(IRepository<ApiKey> apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }

        public async Task<bool> VerifyApiKey(ApiKey apiKey)
        {
			try
			{
				// Attempt to find api key
				ApiKey? foundApiKey = await _apiKeyRepository.GetByIdAsync(apiKey.Id);
				if (foundApiKey == null)
				{
					throw new ApiKeyNotFoundException("Api Key not found");
				}

				// Validate api key
				if (foundApiKey.Value != apiKey.Value)
				{
					throw new ApiKeyNotValidException("Api Key not valid");
				}
				return true;
			}
			catch (ApiKeyNotFoundException)
			{
				throw;
			}
			catch (ApiKeyNotValidException)
			{
				throw;
			}
        }
    }
}
