using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.CustomValidationExceptions;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitorApiDataAccess.Repositories.Concretes
{
    public class MonitoringDeviceRepository : BaseRepository<MonitoringDevice>
    {
        public MonitoringDeviceRepository(BabyMonitorContext context) : base(context)
        {
        }

        public override async Task<MonitoringDevice> PostAsync(MonitoringDevice device)
        {
            MonitoringDevice? foundDevice = await _context.MonitoringDevices.FirstOrDefaultAsync(md => md.Id == device.Id);

            if (foundDevice != null)
            {
                throw new EmailAlreadyInUseException("Email already in use");
            }

            _context.MonitoringDevices.Add(device);
            await _context.SaveChangesAsync();
            return device;
        }

        public override async Task<MonitoringDevice?> GetByIdAsync(Guid? id)
        {
            var foundDevice = await _context.MonitoringDevices
                .Include(md => md._Baby)
                .FirstOrDefaultAsync(md => md.Id == id);

            if (foundDevice == null)
            {
                throw new MonitoringDeviceNotFoundException("Monitoring device not found");
            }

            return foundDevice;
        }

        public override async Task<IEnumerable<MonitoringDevice>> GetAllAsync()
        {
            return await _context.MonitoringDevices.Include(md => md._Baby).ToListAsync();
        }
    }
}
