using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Dtos.BabyState
{
    public class BabyStateSleepIntervalDto
    {
        public bool Awake { get; set; }
        public List<string> Emotions { get; set; }
        public double Percantage { get; set; }
        public DateTime StateStarted { get; set; }
    }
}
