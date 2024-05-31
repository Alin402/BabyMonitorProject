using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Entities
{
    public class Livestream
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }
        [Required]
        public Guid? BabyId { get; set; }
        public double? Time {  get; set; }
        public Guid? DeviceId { get; set; }
        public DateTime? DateStarted { get; set; }
        public Baby? _Baby { get; set; }
        public ICollection<BabyState> BabyStates { get; set; } = new HashSet<BabyState>();
    }
}
