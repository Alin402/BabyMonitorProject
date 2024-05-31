using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Entities;
using BabyMonitorApiDataAccess.Dtos.Babies;

namespace BabyMonitorApiDataAccess.Dtos.Devices
{
    public class GetMonitoringDeviceDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? UserId { get; set; }
        public Guid? BabyId { get; set; }
        public string? LivestreamUrl { get; set; }
        public string? StreamId { get; set; }
        public Guid? ApiKeyId { get; set; }
        public CreatedBabyDto Baby { get; set; }
    }
}
