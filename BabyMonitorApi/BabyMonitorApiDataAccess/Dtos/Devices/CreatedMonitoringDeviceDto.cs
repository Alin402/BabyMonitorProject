using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Dtos.Devices
{
    public class CreatedMonitoringDeviceDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? UserId { get; set; }
        public Guid? BabyId { get; set; }
        public string? BabyName { get; set; }
        public string? BabyPhotoUrl { get; set; }
    }
}
