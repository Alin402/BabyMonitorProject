using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Entities
{
    public class MonitoringDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string? Name { get; set; }
        [Required]
        public Guid? UserId { get; set; }
        public Guid? BabyId { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        public string? LivestreamUrl { get; set; }
        public string? StreamId { get; set; }
        public User? _User { get; set; }
        public Baby? _Baby { get; set; }
    }
}
