using Savemate.Domain.Enums;
using Savemate.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Savemate.Application.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public TransactionTypeEnum Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? Note { get; set; }

        [Display(Name = "From Account")]
        public int? FromAccountId { get; set; }

        [Display(Name = "To Account")]
        public int? ToAccountId { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        // For dropdowns in Create/Edit views
        public IEnumerable<AccountViewModel>? Accounts { get; set; } 
    }
}
