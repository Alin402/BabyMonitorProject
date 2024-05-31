using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabyMonitorApiDataAccess.Entities;
using Microsoft.AspNetCore.Http;

namespace BabyMonitorApiDataAccess.Dtos.Babies
{
    public class CreateBabyDto
    {
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string? Name { get; set; }
        public IFormFile? ImageToUpload {  get; set; }
        [Required]
        public string? BirthDate { get; set; }
    }
}
