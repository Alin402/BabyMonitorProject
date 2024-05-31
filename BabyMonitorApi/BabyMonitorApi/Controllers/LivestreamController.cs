using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Dtos.Livestream;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyMonitorApi.Controllers
{
    [Route("/api/livestream")]
    public class LivestreamController : Controller
    {
        private ILivestreamService _livestreamService;

        public LivestreamController(ILivestreamService livestreamService)
        {
            _livestreamService = livestreamService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLivestream([FromBody] CreateLivestreamDto createLivestream)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                    Livestream? livestream = await _livestreamService.CreateLivestream(createLivestream);
                    return Ok();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLivestream(Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name)) 
                    {
                        GetLivestreamDto livestream = await _livestreamService.GetLivestream(id, User.Identity.Name);
                        return Ok(livestream);
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }
            catch (LivestreamNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }
            catch (BabyNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }
            catch (LivestreamNotAllowedToAccessException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllLivestreams()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                    {
                        List<GetAllLivestreamsDto>? livestreams = await _livestreamService.GetAllLivestreams(User.Identity.Name);
                        return Ok(livestreams);
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (MonitoringDeviceNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLivestream(Guid id)
        {
            try
            {
                if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                {
                    await _livestreamService.DeleteLivestream(id, User.Identity.Name);
                    return Ok();
                }
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (LivestreamNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (BabyNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (LivestreamNotAllowedToAccessException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpGet("{id}/statistics")]
        public async Task<IActionResult> GetLivestreamStatistics(Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GetStreamStatisticsDto streamStatistics = await _livestreamService.GetLivestreamStatistics(id, User.Identity.Name);
                    return Ok(streamStatistics);
                }
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Identity", ex.Message);
            }
            catch (LivestreamNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (BabyNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("BabyId", ex.Message);
            }
            catch (LivestreamNotAllowedToAccessException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("BabyId", ex.Message);
            }
            catch (NotEnoughLivestreamInformationException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            return BadRequest(ModelState);
        }
    }
}
