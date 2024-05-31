using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Entities;
using BabyMonitorApiDataAccess.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitorApiDataAccess.Repositories.Concretes
{
    public class FactoryMonitoringDeviceRepository : BaseRepository<FactoryMonitoringDevice>
    {
        public FactoryMonitoringDeviceRepository(BabyMonitorContext context) : base(context)
        {
        }

        public override async Task<FactoryMonitoringDevice?> GetByIdAsync(Guid? id)
        {
            try
            {
                Guid deviceId = _context.FactoryMonitoringDevices.First().Id;
                FactoryMonitoringDevice? device = await _context.FactoryMonitoringDevices.FirstOrDefaultAsync(fmd => fmd.Id.Equals(id));
                if (device == null)
                {
                    throw new FactoryMonitoringDeviceNotFoundException("Factory monitoring device not found");
                }
                return device;
            }
            catch (FactoryMonitoringDeviceNotFoundException)
            {
                throw;
            }
        }
    }
}
