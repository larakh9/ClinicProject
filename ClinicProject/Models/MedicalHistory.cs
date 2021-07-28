using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Models
{
    public class MedicalHistory
    {
        [Key]
        [Required]
        public long Id { get; set; }


        [Required]
        public long PatienId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }


        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
