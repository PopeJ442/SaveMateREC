using Savemate.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Savemate.Web.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account name is required")]
        [StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account type is required")]
        public AccountTypeEnum Type { get; set; }    

        [Range(0, double.MaxValue, ErrorMessage = "Initial balance must be a positive number")]
        public decimal InitialBalance { get; set; }


    }
}
