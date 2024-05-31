using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Dtos.BabyState;

namespace BabyMonitorApiDataAccess.Dtos.Livestream
{
    public class GetStreamStatisticsDto
    {
        public Guid Id { get; set; }
        public double StreamDuration { get; set; }
        public double TotalSleepDuration { get; set; }
        public List<CreateBabyStateDto> BabyStates { get; set; }
        public List<BabyStateSleepIntervalDto> BabyStatesSleepIntervals {  get; set; }
        public Dictionary<string, int> EmotionFrequency { get; set; }
    }
}
