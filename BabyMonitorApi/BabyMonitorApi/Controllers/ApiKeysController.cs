using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BabyMonitorApi.Controllers
{
    [Route("api/keys")]
    public class ApiKeysController : Controller
    {
        private IApiKeyService _apiKeyService;

        public ApiKeysController(IApiKeyService apiKeyService)
        {
            _apiKeyService = apiKeyService;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyApiKey([FromBody] ApiKey apiKey)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool verificationResult = await _apiKeyService.VerifyApiKey(apiKey);
                    if (verificationResult)
                    {
                        return Ok();
                    }
                }
            }
            catch (ApiKeyNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (ApiKeyNotValidException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Value", ex.Message);
            }
            return BadRequest(ModelState);
        }
    }
}
