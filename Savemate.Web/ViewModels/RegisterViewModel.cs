using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Savemate.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
        public   string? Country { get; set; }
        public   string? PhoneNumber { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public DateOnly DOB { get; set; }

      
        public string? CountryCode { get; set; }

        public List<SelectListItem> Countries { get; set; } = new();
    }

}
