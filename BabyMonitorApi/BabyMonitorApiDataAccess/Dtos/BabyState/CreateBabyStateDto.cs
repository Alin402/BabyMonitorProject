using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Dtos.BabyState
{
    public class CreateBabyStateDto
    {
        public double? AtSecond {  get; set; }
        public string? Emotion { get; set; }
        public bool Awake { get; set; }
    }
}
