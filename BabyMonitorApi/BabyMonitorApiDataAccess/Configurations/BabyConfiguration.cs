using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace BabyMonitorApiDataAccess.Configurations
{
    public class BabyConfiguration : IEntityTypeConfiguration<Baby>
    {
        public void Configure(EntityTypeBuilder<Baby> builder)
        {
            builder.ToTable("babies");
        }
    }
}
