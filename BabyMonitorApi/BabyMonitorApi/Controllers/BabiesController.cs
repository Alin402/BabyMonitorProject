using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Dtos.Babies;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyMonitorApi.Controllers
{
    [Authorize]
    [Route("api/baby")]
    public class BabiesController : Controller
    {
        private IBabyService _babyService;

        public BabiesController(IBabyService babyService)
        {
            _babyService = babyService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBaby([FromForm] CreateBabyDto createBaby)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                    {
                        var createdBaby = await _babyService.CreateBaby(createBaby, User.Identity.Name);
                        return Ok(createdBaby);
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (UploadBabyPictureException ex)
            {
                ModelState.AddModelError("ImageToUpload", ex.Message);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllBabies()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                    {
                        List<CreatedBabyDto> babies = await _babyService.GetAllBabies(User.Identity.Name);
                        return Ok(babies);
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBaby(Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                    {
                        CreatedBabyDto? baby = await _babyService.GetBaby(id, User.Identity.Name);
                        return Ok(baby);
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("UserId", ex.Message);
                Console.WriteLine(ex);
            }
            catch (BabyNotAllowedToAccessException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }
            catch (BabyNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBaby(Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                    {
                        await _babyService.DeleteBaby(id, User.Identity.Name);
                        return Ok();
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("UserId", ex.Message);
                Console.WriteLine(ex);
            }
            catch (BabyNotAllowedToAccessException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }
            catch (BabyNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBaby(Guid id, [FromForm] UpdateBabyDto updateBaby)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                    {
                        CreatedBabyDto baby =  await _babyService.UpdateBaby(id, updateBaby, User.Identity.Name);
                        return Ok(baby);
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("UserId", ex.Message);
                Console.WriteLine(ex);
            }
            catch (BabyNotAllowedToAccessException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }
            catch (UploadBabyPictureException ex)
            {
                ModelState.AddModelError("ImageToUpload", ex.Message);
                Console.WriteLine(ex);
            }
            catch (BabyNotFoundException ex)
            {
                ModelState.AddModelError("Id", ex.Message);
                Console.WriteLine(ex);
            }

            return BadRequest(ModelState);
        }
    }
}
