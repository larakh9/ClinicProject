using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Models
{
    public class Typee
    {
        [Key]
        [Required]
        public long Id { get; set; }


        [StringLength(100, ErrorMessage = "Type is too long! Must be {1} or less")]
        public string AppointmentType { get; set; }


    }
}
