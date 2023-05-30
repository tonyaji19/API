using API.Utility;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels.Accounts
{
    public class RegisterVM
    {
        /*        public string? NIK { get; set; }
        */
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTime BirthDate { get; set; }
        public GenderLevel Gender { get; set; }

        public DateTime HiringDate { get; set; }
        [EmailAddress]
        [EmailPhoneValidation(nameof(Email))]
        public string Email { get; set; }

        [Phone]
        [EmailPhoneValidation(nameof(PhoneNumber))]
        public string PhoneNumber { get; set; }

        public string Major { get; set; }

        public string Degree { get; set; }

        [Range(0, 4, ErrorMessage = "GPA must be between 0 - 4")]
        public float GPA { get; set; }

        //public Guid UniversityGuid { get; set; }
        [Display(Name = "University Code")]
        public string UniversityCode { get; set; }
        [Display(Name = "University Name")]
        public string UniversityName { get; set; }

        [PasswordValidation(ErrorMessage = "Password must contain at least one number, one uppercase letter, one lowercase letter, one symbol, and be at least 6 characters long")]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        // public University? University { get; set; }


    }
}