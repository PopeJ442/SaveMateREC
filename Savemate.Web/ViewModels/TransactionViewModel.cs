using Savemate.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Savemate.Application.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Transaction Type")]
        public TransactionTypeEnum Type { get; set; } = TransactionTypeEnum.Expense;

        [Required]
        [Display(Name = "Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Note")]
        public string? Note { get; set; }

        [Display(Name = "From Account")]
        public int? FromAccountId { get; set; }

        [Display(Name = "To Account")]
        public int? ToAccountId { get; set; }

        [Display(Name = "Category")]
        public CategoryTypeEnum Category { get; set; }

        // --- Dropdowns ---
        public IEnumerable<SelectListItem>? Accounts { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }

        // --- Convenience properties ---
        public string? FromAccountName { get; set; }
        public string? ToAccountName { get; set; }
        public string? CategoryName { get; set; }
    }
}
