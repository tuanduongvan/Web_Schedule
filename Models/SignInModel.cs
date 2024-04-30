using System.ComponentModel.DataAnnotations;

namespace PBL_WEB.Models
{
    public class SignInModel
    {
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "UserName not empty")]
        public string UserName { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password not empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
