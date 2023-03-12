using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Contact : BaseEntity
    {
        [MaxLength(30, ErrorMessage = "Name is too long")]
        [MinLength(2, ErrorMessage = "Name is too short")]
        [Required(ErrorMessage = "Name in required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DisplayName("Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Is married")]
        public bool Married { get; set; }

        [MaxLength(25, ErrorMessage = "Phone number is too long")]
        [MinLength(5, ErrorMessage = "Phone number is too short")]
        [Required(ErrorMessage = "Phone number is required")]
        [DisplayName("Phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        public decimal Salary { get; set; }
    }
}
