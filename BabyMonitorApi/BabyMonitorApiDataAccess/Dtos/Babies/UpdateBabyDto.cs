using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BabyMonitorApiDataAccess.Dtos.Babies
{
    public class UpdateBabyDto
    {
        [Required]
        public string? Name { get; set; }
        public IFormFile? ImageToUpload { get; set; }
        [Required]
        public string? BirthDate { get; set; }
    }
}
