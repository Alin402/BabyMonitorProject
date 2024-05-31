using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiBusiness.Abstractions;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Dtos;
using BabyMonitorApiDataAccess.Dtos.Devices;
using BabyMonitorApiDataAccess.Entities;
using BabyMonitorApiDataAccess.Repositories.Abstractions;

namespace BabyMonitorApiBusiness.Concretes
{
    public class DeviceService : IDeviceService
    {
        private IRepository<MonitoringDevice> _monitoringDeviceRepository;
        private IRepository<FactoryMonitoringDevice> _factoryMonitoringDeviceRepository;
        private IRepository<Baby> _babyRepository;
        private IUserRepository _userRepository;
        private IRepository<User> _userBaseRepository;
        private IRepository<ApiKey> _apiKeyRepository;

        public DeviceService(IRepository<MonitoringDevice> monitoringDeviceRepository, IRepository<FactoryMonitoringDevice> factoryMonitoringDeviceRepository, IRepository<Baby> babyRepository, IUserRepository userRepository, IRepository<ApiKey> apiKeyRepository)
        {
            _monitoringDeviceRepository = monitoringDeviceRepository;
            _factoryMonitoringDeviceRepository = factoryMonitoringDeviceRepository;
            _babyRepository = babyRepository;
            _userRepository = userRepository;
            _apiKeyRepository = apiKeyRepository;
        }

        public async Task<CreatedMonitoringDeviceDto> UpdateMonitoringDevice(Guid id, string userEmail, UpdateMonitoringDeviceDto updateMonitoringDevice)
        {
            try
            {
                MonitoringDevice? foundDevice = await _monitoringDeviceRepository.GetByIdAsync(id);

                if (foundDevice != null)
                {
                    User? connectedUser = await _userRepository.FindByEmailAsync(userEmail);
                    if (connectedUser == null)
                    {
                        throw new UserNotFoundException("User not found");
                    }

                    if (!foundDevice.UserId.Equals(connectedUser.Id))
                    {
                        throw new DeviceNotAllowedToAccessException("Device not allowed to access");
                    }

                    foundDevice.Name = updateMonitoringDevice.Name;
                    foundDevice.BabyId = updateMonitoringDevice.BabyId;

                    MonitoringDevice updatedMonitoringDevice = await _monitoringDeviceRepository.UpdateAsync(foundDevice);
                    if (updatedMonitoringDevice != null)
                    {
                        return new CreatedMonitoringDeviceDto()
                        {
                            Id = updatedMonitoringDevice.Id,
                            Name = updatedMonitoringDevice.Name,
                            BabyId = updatedMonitoringDevice.BabyId,
                            UserId = updatedMonitoringDevice.UserId,
                            BabyName = updatedMonitoringDevice._Baby?.Name,
                            BabyPhotoUrl = updatedMonitoringDevice._Baby?.PhotoUrl,
                        };
                    }
                }
                else
                {
                    throw new MonitoringDeviceNotFoundException("Monitoring device not found");
                }
            }
            catch (MonitoringDeviceNotFoundException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (DeviceNotAllowedToAccessException)
            {
                throw;
            }
            return null;
        }

        public async Task DeleteMonitoringDevice(Guid id, string userEmail)
        {
            try
            {
                MonitoringDevice? foundDevice = await _monitoringDeviceRepository.GetByIdAsync(id);

                if (foundDevice != null)
                {
                    User? connectedUser = await _userRepository.FindByEmailAsync(userEmail);
                    if (connectedUser == null)
                    {
                        throw new UserNotFoundException("User not found");
                    }

                    if (!foundDevice.UserId.Equals(connectedUser.Id))
                    {
                        throw new DeviceNotAllowedToAccessException("Device not allowed to access");
                    }

                    // Make factory monitoring device not active and delete monitoring device
                    FactoryMonitoringDevice? factoryMonitoringDevice = await _factoryMonitoringDeviceRepository.
                    GetByIdAsync(id);

                    if (factoryMonitoringDevice == null)
                    {
                        throw new FactoryMonitoringDeviceNotFoundException("Factory monitoring device not found");
                    }

                    if (factoryMonitoringDevice.IsActive)
                    {
                        factoryMonitoringDevice.IsActive = false;
                    }

                    await _monitoringDeviceRepository.DeleteAsync(id);
                }
                else
                {
                    throw new MonitoringDeviceNotFoundException("Monitoring device not found");
                }
            }
            catch (MonitoringDeviceNotFoundException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (DeviceNotAllowedToAccessException)
            {
                throw;
            }
            catch (FactoryMonitoringDeviceNotFoundException)
            {
                throw;
            }
        }

        public async Task<List<CreatedMonitoringDeviceDto>> GetAllMonitoringDevices(string userEmail)
        {
            try
            {
                if (!string.IsNullOrEmpty(userEmail))
                {
                    User? user = await _userRepository.FindByEmailAsync(userEmail);

                    if (user != null)
                    {
                        Console.WriteLine(user.Id);
                        return _monitoringDeviceRepository.
                            GetAllAsync().
                            Result.
                            Where(md => md.UserId == user.Id).
                            Select(md => new CreatedMonitoringDeviceDto()
                            {
                                Id = md.Id,
                                Name = md.Name,
                                UserId = md.UserId,
                                BabyId = md.BabyId,
                                BabyName = md._Baby?.Name,
                                BabyPhotoUrl = md._Baby?.PhotoUrl,
                            }).
                            ToList();
                    }

                    throw new UserNotFoundException("User not found");
                }
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            return new List<CreatedMonitoringDeviceDto>();
        }

        public async Task<GetMonitoringDeviceDto> GetMonitoringDevice(Guid id, string userEmail)
        {
            try
            {
                MonitoringDevice? foundDevice = await _monitoringDeviceRepository.GetByIdAsync(id);

                if (foundDevice != null)
                {
                    User? connectedUser = await _userRepository.FindByEmailAsync(userEmail);
                    if (connectedUser == null)
                    {
                        throw new UserNotFoundException("User not found");
                    }

                    if (!foundDevice.UserId.Equals(connectedUser.Id))
                    {
                        throw new DeviceNotAllowedToAccessException("Device not allowed to access");
                    }

                    return new GetMonitoringDeviceDto()
                    {
                        Id = foundDevice.Id,
                        Name = foundDevice.Name,
                        UserId= foundDevice.UserId,
                        BabyId = foundDevice.BabyId,
                        StreamId = foundDevice.StreamId,
                        Baby = new BabyMonitorApiDataAccess.Dtos.Babies.CreatedBabyDto()
                        {
                            Id = foundDevice._Baby.Id,
                            Name = foundDevice._Baby.Name,
                            PhotoUrl = foundDevice._Baby.PhotoUrl,
                            UserId = foundDevice._Baby.UserId,
                        }
                    };
                }
                throw new MonitoringDeviceNotFoundException("Monitoring device not found");
            }
            catch (MonitoringDeviceNotFoundException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (DeviceNotAllowedToAccessException)
            {
                throw;
            }
            return null;
        }

        public async Task<GetMonitoringDeviceDto> DeviceGetMonitoringDevice(DeviceGetMonitoringDeviceDto getMonitoringDevice)
        {
            try
            {
                MonitoringDevice? foundDevice = await _monitoringDeviceRepository.GetByIdAsync(getMonitoringDevice.DeviceId);

                if (foundDevice != null)
                {
                    // Very if api key exists
                    ApiKey? apiKey = await _apiKeyRepository.GetByIdAsync(getMonitoringDevice.ApiKeyId);
                    if (apiKey == null)
                    {
                        throw new ApiKeyNotFoundException("Api key not found");
                    }

                    if (!string.IsNullOrEmpty(apiKey.Value) && !apiKey.Value.Equals(getMonitoringDevice.ApiKeyValue))
                    {
                        throw new ApiKeyNotValidException("Api key not valid");
                    }

                    return new GetMonitoringDeviceDto()
                    {
                        Id = foundDevice.Id,
                        Name = foundDevice.Name,
                        UserId = foundDevice.UserId,
                        BabyId = foundDevice.BabyId,
                        StreamId = foundDevice.StreamId,
                        LivestreamUrl = foundDevice.LivestreamUrl
                    };
                }
                throw new MonitoringDeviceNotFoundException("Monitoring device not found");
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }

        public async Task<GetMonitoringDeviceDto> DeviceUpdateMonitoringDevice(DeviceUpdateMonitoringDeviceDto getMonitoringDevice)
        {
            try
            {
                MonitoringDevice? foundDevice = await _monitoringDeviceRepository.GetByIdAsync(getMonitoringDevice.DeviceId);

                if (foundDevice != null)
                {
                    // Very if api key exists
                    ApiKey? apiKey = await _apiKeyRepository.GetByIdAsync(getMonitoringDevice.ApiKeyId);
                    if (apiKey == null)
                    {
                        throw new ApiKeyNotFoundException("Api key not found");
                    }

                    if (!string.IsNullOrEmpty(apiKey.Value) && !apiKey.Value.Equals(getMonitoringDevice.ApiKeyValue))
                    {
                        throw new ApiKeyNotValidException("Api key not valid");
                    }

                    foundDevice.LivestreamUrl = getMonitoringDevice.LivestreamUrl;
                    foundDevice.StreamId = getMonitoringDevice.StreamId;
                    await _monitoringDeviceRepository.UpdateAsync(foundDevice);

                    return new GetMonitoringDeviceDto()
                    {
                        Id = foundDevice.Id,
                        Name = foundDevice.Name,
                        UserId = foundDevice.UserId,
                        BabyId = foundDevice.BabyId,
                    };
                }
                throw new MonitoringDeviceNotFoundException("Monitoring device not found");
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }


        public async Task<MonitoringDevice> RegisterMonitoringDevice(CreateMonitoringDeviceDto createMonitoringDevice, string userEmail)
        {
            try
            {
                FactoryMonitoringDevice? factoryMonitoringDevice = await _factoryMonitoringDeviceRepository.
                    GetByIdAsync(createMonitoringDevice.Id);

                if (factoryMonitoringDevice == null)
                {
                    throw new FactoryMonitoringDeviceNotFoundException("Factory monitoring device not found");
                }

                if (factoryMonitoringDevice.IsActive)
                {
                    throw new FactoryMonitoringDeviceAlreadyActiveException("The monitoring device is already active");
                }

                User? user = await _userRepository.FindByEmailAsync(userEmail);
                
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                Guid babyId = createMonitoringDevice.BabyId;
                Baby? baby = await _babyRepository.GetByIdAsync(babyId);

                if (baby == null)
                {
                    throw new BabyNotFoundException("Baby not found");
                }

                MonitoringDevice newMonitoringDevice = new MonitoringDevice()
                {
                    Id = factoryMonitoringDevice.Id,
                    Name = createMonitoringDevice.Name,
                    UserId = user.Id,
                    BabyId = createMonitoringDevice.BabyId,
                };

                factoryMonitoringDevice.IsActive = true;

                MonitoringDevice? createdMonitoringDevice = await _monitoringDeviceRepository.PostAsync(newMonitoringDevice);

                if (createdMonitoringDevice != null)
                {
                    await _factoryMonitoringDeviceRepository.UpdateAsync(factoryMonitoringDevice);
                    return createdMonitoringDevice;
                }

                return null;
            }
            catch (FactoryMonitoringDeviceNotFoundException)
            {
                throw;
            }
            catch (FactoryMonitoringDeviceAlreadyActiveException)
            {
                throw;
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch (BabyNotFoundException)
            {
                throw;
            }
        }

        public async Task<GetDeviceIdListDto> GetDeviceIdList(string serverKey, Guid userId)
        {
            try
            {
                if (serverKey != "85159608637075381844")
                {
                    throw new ServerKeyNotValidException("Server key not valid");
                }

                User? user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new UserNotFoundException("User not found");
                }

                // Find monitoring devices associated to user
                List<Guid> deviceIds = _monitoringDeviceRepository.
                    GetAllAsync().
                    Result.
                    ToList().
                    Where(md => md.UserId.Equals(userId)).
                    Select(mt => mt.Id).
                    ToList();

                return new GetDeviceIdListDto
                {
                    MonitoringDeviceKeys = deviceIds
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
