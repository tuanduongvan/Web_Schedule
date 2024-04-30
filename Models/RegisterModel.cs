
using System.ComponentModel.DataAnnotations;

namespace PBL_WEB.Models
{
    public class RegisterModel
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
        public string ConfirmPassword {  get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number not empty")]
        public string PhoneNumber { get; set; }

        [Display(Name = "name")]
        [Required(ErrorMessage = "Username not empty")]
        public string UserName {  get; set; }


        [Display(Name = "FullName")]
        [Required(ErrorMessage = "FullName not empty")]
        public string FullName { get; set; }
        [Display(Name = "Addrees")]
        [Required(ErrorMessage = "Address not empty")]
        public string Address { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Dayofbirth")]
        [Required(ErrorMessage = "DayOfBirth not empty")]
        public DateTime DayOfBirth { get; set; }
    }
}
