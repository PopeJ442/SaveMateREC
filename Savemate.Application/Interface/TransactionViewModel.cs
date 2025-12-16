using Savemate.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

       
        public IEnumerable<SelectListItem>? Accounts { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }

      
        public string? FromAccountName { get; set; }
        public string? ToAccountName { get; set; }
        public string? CategoryName { get; set; }

        
        public bool IsReversed { get; set; } = false;
        public bool IsReversalEntry { get; set; } = false;


        public int? ParentTransactionId { get; set; }

        public IEnumerable<string> ValidateTransaction()
        {
            var errors = new List<string>();

            switch (Type)
            {
                case TransactionTypeEnum.Income:
                    if (ToAccountId == null)
                        errors.Add("To Account is required for Income transactions.");
                    if (FromAccountId != null)
                        errors.Add("Income should not have a From Account.");
                    break;

                case TransactionTypeEnum.Expense:
                    if (FromAccountId == null)
                        errors.Add("From Account is required for Expense transactions.");
                    if (ToAccountId != null)
                        errors.Add("Expense should not have a To Account.");
                    break;

                case TransactionTypeEnum.Transfer:
                    if (FromAccountId == ToAccountId)
                        errors.Add("You cannot transfer to the same account.");
                    if (FromAccountId == null || ToAccountId == null)
                        errors.Add("Transfer requires both From and To accounts.");
                    break;
            }

            return errors;
        }

    }

}
