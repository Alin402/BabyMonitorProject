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
    public class LivestreamConfiguration : IEntityTypeConfiguration<Livestream>
    {
        public void Configure(EntityTypeBuilder<Livestream> builder)
        {
            builder.ToTable("livestream");
            builder.
                HasOne<Baby>(l => l._Baby).
                WithMany(b => b.Livestreams);
        }
    }
}
