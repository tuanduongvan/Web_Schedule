using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PBL_WEB.Models
{
    public class CustomerEdit
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Phonenumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]

        public string Address { get; set; }
        [Required]
        //[DisplayFormat()]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DayofBirth { get; set; }
        [Required]
        public string AcountId { get; set; }
    }
}
