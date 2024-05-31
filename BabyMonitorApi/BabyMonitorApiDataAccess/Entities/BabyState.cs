using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Entities
{
    public class BabyState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }
        [Required]
        public Guid? LivestreamId { get; set; }
        [Required]
        public double? AtSecond { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string? Emotion { get; set; }
        public bool Awake { get; set; }
        public Livestream? _Livestream { get; set; }
    }
}
