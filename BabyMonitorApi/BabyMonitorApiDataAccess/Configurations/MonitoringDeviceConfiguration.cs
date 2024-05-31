using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BabyMonitorApiDataAccess.Configurations
{
    public class MonitoringDeviceConfiguration : IEntityTypeConfiguration<MonitoringDevice>
    {
        public void Configure(EntityTypeBuilder<MonitoringDevice> builder)
        {
            builder.ToTable("monitoring_devices");
            builder.HasOne<Baby>(md => md._Baby).WithMany();
        }
    }
}
