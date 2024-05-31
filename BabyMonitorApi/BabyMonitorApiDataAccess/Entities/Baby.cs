using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Entities
{
    public class Baby
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid? UserId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string? Name { get; set; }
        [Column(TypeName = "nvarchar(300)")]
        public string? PhotoUrl { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public User? _User { get; set; }
        public ICollection<Livestream> Livestreams { get; set; } = new HashSet<Livestream>();
    }
}
