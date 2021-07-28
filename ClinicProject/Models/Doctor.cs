using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicProject.Models
{
    public class Doctor
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


        [StringLength(100, ErrorMessage = "Address length must be less than {1}")]
        public string Address { get; set; }


        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }


        [DataType(DataType.PhoneNumber)]
        public string PhoneNum { get; set; }


        [DataType(DataType.Currency)]
        public Decimal? MonthlySalary { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [RegularExpression("^([A-Z]{2}[ '+'\\'+'-]?[0-9]{2})(?=(?:[ '+'\\'+'-]?[A-Z0-9]){9,30}$)((?:[ '+'\\'+'-]?[A-Z0-9]{3,5}){2,7})([ '+'\\'+'-]?[A-Z0-9]{1,3})?$")]
        public string IBAN { get; set; }

        public IList<Appointment> Appointments { get; set; }


        [Required]
        public long SpecializationId { get; set; }


        [ForeignKey("SpecializationId")]
        public Specialization Specialization { get; set; }

        public string Country { get; set; }

    
        public string DoctorName
        {
            get { return $"{FirstName} {LastName}"; }
            set { value= $"{FirstName} {LastName}"; }
        }

    }
}
