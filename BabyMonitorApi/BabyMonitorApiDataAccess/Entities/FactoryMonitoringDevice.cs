using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Entities
{
    public class FactoryMonitoringDevice
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
