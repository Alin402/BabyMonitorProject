using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string? Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string? Username { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [EmailAddress]
        public string? Email { get; set; }
        [Column(TypeName = "nvarchar(200)")]
        [MinLength(8)]
        public string? Password { get; set; }
        public ICollection<MonitoringDevice> MonitoringDevices { get; set; } = new HashSet<MonitoringDevice>();
        public ICollection<Baby> Babies { get; set; } = new HashSet<Baby>();
    }
}
