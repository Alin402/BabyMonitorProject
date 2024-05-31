using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Dtos;
using BabyMonitorApiDataAccess.Dtos.Devices;
using BabyMonitorApiDataAccess.Entities;

namespace BabyMonitorApiBusiness.Abstractions
{
    public interface IDeviceService
    {
        Task<MonitoringDevice> RegisterMonitoringDevice(CreateMonitoringDeviceDto createMonitoringDevice, string userEmail);
        Task<List<CreatedMonitoringDeviceDto>> GetAllMonitoringDevices(string userEmail);
        Task<GetMonitoringDeviceDto> GetMonitoringDevice(Guid id, string userEmail);
        Task DeleteMonitoringDevice(Guid id, string userEmail);
        Task<CreatedMonitoringDeviceDto> UpdateMonitoringDevice(Guid id, string userEmail, UpdateMonitoringDeviceDto updateMonitoringDevice);
        Task<GetMonitoringDeviceDto> DeviceGetMonitoringDevice(DeviceGetMonitoringDeviceDto getMonitoringDevice);
        Task<GetMonitoringDeviceDto> DeviceUpdateMonitoringDevice(DeviceUpdateMonitoringDeviceDto getMonitoringDevice);
        Task<GetDeviceIdListDto> GetDeviceIdList(string serverKey, Guid userId);
    }
}
