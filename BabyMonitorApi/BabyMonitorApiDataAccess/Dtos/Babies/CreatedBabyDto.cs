using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyMonitorApiDataAccess.Dtos.Babies
{
    public class CreatedBabyDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Name { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
