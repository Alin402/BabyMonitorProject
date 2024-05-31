using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Configurations;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BabyMonitorApiDataAccess
{
    public class BabyMonitorContext : DbContext
    {
        public BabyMonitorContext(DbContextOptions<BabyMonitorContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MonitoringDevice> MonitoringDevices { get; set; }
        public DbSet<Baby> Babies { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<FactoryMonitoringDevice> FactoryMonitoringDevices { get; set; }
        public DbSet<Livestream> Livestreams { get; set; }
        public DbSet<BabyState> BabyStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UserConfiguration().Configure(modelBuilder.Entity<User>());
            new MonitoringDeviceConfiguration().Configure(modelBuilder.Entity<MonitoringDevice>());
            new BabyConfiguration().Configure(modelBuilder.Entity<Baby>());
            new ApiKeyConfiguration().Configure(modelBuilder.Entity<ApiKey>());
            new FactoryMonitoringDeviceConfiguration().Configure(modelBuilder.Entity<FactoryMonitoringDevice>());
            new LivestreamConfiguration().Configure(modelBuilder.Entity<Livestream>());
            new BabyStateConfiguration().Configure(modelBuilder.Entity<BabyState>());
        }
    }
}
