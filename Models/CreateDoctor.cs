using System.ComponentModel.DataAnnotations;

namespace PBL_WEB.Models
{
    public class CreateDoctor
    {
        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "Email is not empty")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is not empty")]
        public string Password { get; set; }

        [Display(Name = "Repeat Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Not right")]
        [Required(ErrorMessage = "Password is not empty")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number not empty")]
        public string PhoneNumber { get; set; }

        [Display(Name = "name")]
        [Required(ErrorMessage = "Username not empty")]
        public string UserName { get; set; }

        [Display(Name = "Specialize")]
        [Required(ErrorMessage = "FullName not empty")]
        public string specialize { get; set; }

        [Display(Name = "FullName")]
        [Required(ErrorMessage = "FullName not empty")]
        public string FullName { get; set; }
        [Display(Name = "Addrees")]
        [Required(ErrorMessage = "Address not empty")]
        public string Address { get; set; }

        [Display(Name = "Discripsion")]
        [Required(ErrorMessage = "Discripsion not empty")]
        public string Discripsion { get; set; }

        [Display(Name = "ImageFile")]
        [Required(ErrorMessage = "ImageFile not empty")]
        public IFormFile? ImageFile { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Dayofbirth")]
        [Required(ErrorMessage = "DayOfBirth not empty")]
        public DateTime DayOfBirth { get; set; }
    }
}
