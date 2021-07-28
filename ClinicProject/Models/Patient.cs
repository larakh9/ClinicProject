using ClinicProject.Controllers;
using ClinicProject.NewFolder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Models
{
    public class Patient
    {
        [Key]
        [Required]
        public long Id { get; set; }


        [StringLength(50, ErrorMessage = "Name length must be less than {1}")]
        [RegularExpression("^[A-Za-z]+$")]
        public string FirstName { get; set; }


        [StringLength(50, ErrorMessage = "Name length must be less than {1}")]
        [RegularExpression("^[A-Za-z]+$")]
        public string LastName { get; set; }



        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }



        public string Gender { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string PhoneNum { get; set; }



        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



        [StringLength(100, ErrorMessage = "Address length must be less than {1}")]
        public string Address { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime RegDate { get; set; }


        [RegularExpression("^\\d{9}$")]
        public string SSN { get; set; }

        public int Age
        {
            get {
                var temp = DateTime.Today;
                var age = temp.Year - DOB.Year;
                return age;
            }
           
        }

        public List<Appointment> Appointments { get; set; }
        public List<MedicalHistory> MedicalHistories { get; set; }

        public string Country { get; set; }

        public string PatientName
        {
            get { return $"{FirstName} {LastName}"; }
            set { value = $"{FirstName} {LastName}"; }
        }
    }
}
