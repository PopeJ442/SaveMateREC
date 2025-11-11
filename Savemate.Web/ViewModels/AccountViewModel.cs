using Savemate.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Savemate.Web.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account name is required")]
        [StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters")]
        [Display(Name = "Account Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account type is required")]
        [Display(Name = "Account Type")]
        public AccountTypeEnum Type { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Initial balance must be a positive number")]
        [Display(Name = "Initial Balance")]
        public decimal InitialBalance { get; set; }

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
        [Display(Name = "Description (Optional)")]
        public string? Description { get; set; }

        [Display(Name = "User ID")]
        public string? UserId { get; set; }

        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
