using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Dtos.BabyState;

namespace BabyMonitorApiDataAccess.Dtos.Livestream
{
    public class CreateLivestreamDto
    {
        public Guid DeviceId { get; set; }
        public double? Time {  get; set; }
        public string? DateStarted { get; set; }
        public string? ServerKey { get; set; }
        public List<CreateBabyStateDto>? BabyStates { get; set; }
    }
}
