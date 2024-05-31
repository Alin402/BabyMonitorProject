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
    public class BabyStateConfiguration : IEntityTypeConfiguration<BabyState>
    {
        public void Configure(EntityTypeBuilder<BabyState> builder)
        {
            builder.ToTable("baby_state");
            builder.
                HasOne<Livestream>(bs => bs._Livestream).
                WithMany(l => l.BabyStates);
        }
    }
}
