using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Dtos;
using BabyMonitorApiDataAccess.Dtos.Devices;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BabyMonitorApi.Controllers
{
    [Route("api/device")]
    public class DevicesController : Controller
    {
        private IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterMonitoringDevice([FromBody] CreateMonitoringDeviceDto createMonitoringDevice)
        {
            try
            {
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    MonitoringDevice? createdMonitoringDevice = await _deviceService.RegisterMonitoringDevice(createMonitoringDevice, User.Identity.Name);
                    Console.WriteLine(createdMonitoringDevice);

                    if (createdMonitoringDevice != null)
                    {
                        CreatedMonitoringDeviceDto createdMonitoringDeviceDto = new()
                        {
                            Id = createdMonitoringDevice.Id,
                            Name = createdMonitoringDevice.Name,
                            UserId = createdMonitoringDevice.Id,
                            BabyId = createdMonitoringDevice.BabyId
                        };
                        return Ok(createdMonitoringDeviceDto);
                    }
                }
            }
            catch (FactoryMonitoringDeviceNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (FactoryMonitoringDeviceAlreadyActiveException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("UserId", ex.Message);
            }
            catch (BabyNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("BabyId", ex.Message);
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMonitoringDevices()
        {
            try
            {
                if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                {
                    List<CreatedMonitoringDeviceDto> devices = await _deviceService.
                        GetAllMonitoringDevices(User.Identity.Name);

                    return Ok(devices);
                }
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMonitoringDevice(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    if (User.Identity != null && ! string.IsNullOrEmpty(User.Identity.Name))
                    {
                        GetMonitoringDeviceDto? device = await _deviceService.GetMonitoringDevice(id, User.Identity.Name);
                        if (device != null)
                        {
                            return Ok(device);
                        }
                    }
                }
            }
            catch (MonitoringDeviceNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (DeviceNotAllowedToAccessException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonitoringDevice(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                    {
                        await _deviceService.DeleteMonitoringDevice(id, User.Identity.Name);
                        return Ok();
                    }
                }
            }
            catch (MonitoringDeviceNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (DeviceNotAllowedToAccessException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (FactoryMonitoringDeviceNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMonitoringDevice(Guid id, [FromBody] UpdateMonitoringDeviceDto updateMonitoringDevice)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    if (User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
                    {
                        CreatedMonitoringDeviceDto createdMonitoringDeviceDto = await _deviceService.UpdateMonitoringDevice(id, User.Identity.Name, updateMonitoringDevice);
                        if (createdMonitoringDeviceDto != null) 
                        {
                            return Ok(createdMonitoringDeviceDto);
                        }
                    }
                }
            }
            catch (MonitoringDeviceNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            catch (DeviceNotAllowedToAccessException ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError("Id", ex.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("get/key")]
        public async Task<IActionResult> DeviceGetMonitoringDevice([FromBody] DeviceGetMonitoringDeviceDto monitoringDevice)
        {
            try
            {
                GetMonitoringDeviceDto device = await _deviceService.DeviceGetMonitoringDevice(monitoringDevice);
                if (device != null)
                {
                    return Ok(device);
                }
                return BadRequest();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPut("get/key")]
        public async Task<IActionResult> DeviceUpdateMonitoringDevice([FromBody] DeviceUpdateMonitoringDeviceDto monitoringDevice)
        {
            try
            {
                GetMonitoringDeviceDto device = await _deviceService.DeviceUpdateMonitoringDevice(monitoringDevice);
                if (device != null)
                {
                    return Ok(device);
                }
                return BadRequest();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get/ids/{id}")]
        public async Task<IActionResult> GetMonitoringDeviceIdList(Guid id, string serverKey)
        {
            try
            {
                GetDeviceIdListDto deviceList = await _deviceService.GetDeviceIdList(serverKey, id);
                return Ok(deviceList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return BadRequest();
        }
    }
}
