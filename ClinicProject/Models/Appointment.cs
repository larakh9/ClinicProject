﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Models
{
    public class Appointment
    {
        [Key]
        [Required]
        public long Id { get; set; }


        [Required]
        public long DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }


        [Required]
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime Reservation { get; set; }


        public long TypeId { get; set; }


        [ForeignKey("TypeId")]
        public Typee Type { get; set; }

    }
}
