using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Dtos.Babies;
using BabyMonitorApiDataAccess.Dtos.BabyState;
using BabyMonitorApiDataAccess.Entities;

namespace BabyMonitorApiDataAccess.Dtos.Livestream
{
    public class GetLivestreamDto
    {
        public Guid? Id { get; set; }
        public Guid? BabyId { get; set; }
        public double? Time { get; set; }
        public DateTime? DateStarted { get; set; }
        public List<CreateBabyStateDto>? BabyStates { get; set; }
        public CreatedBabyDto Baby { get; set; }
    }
}
