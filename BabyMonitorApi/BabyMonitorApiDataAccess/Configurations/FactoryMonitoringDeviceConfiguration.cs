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
    public class FactoryMonitoringDeviceConfiguration : IEntityTypeConfiguration<FactoryMonitoringDevice>
    {
        public void Configure(EntityTypeBuilder<FactoryMonitoringDevice> builder)
        {
            builder.ToTable("factorymonitoringdevice");
        }
    }
}
