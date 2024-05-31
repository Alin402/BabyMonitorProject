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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users").HasKey(u => u.Id);

            builder.
                HasMany<MonitoringDevice>(u => u.MonitoringDevices).
                WithOne(md => md._User);
            builder.
                HasMany<Baby>(u => u.Babies).
                WithOne(b => b._User);
        }
    }
}
