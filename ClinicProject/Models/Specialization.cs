using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Models
{
    public class Specialization
    {
        [Key]
        [Required]
        public long Id { get; set; }


        [StringLength(50, ErrorMessage ="Name is too long! Must be {1} or less")]
        [Display (Name ="Specialization Name")]
        public string SpecializationName { get; set; }

        public IList<Doctor> Doctors { get; set; }
    }
}
