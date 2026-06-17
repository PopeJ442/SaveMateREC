using Savemate.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Savemate.Application.ViewModels
{
    public class TransactionDTO
    {
        public int Id { get; set; } 
        public TransactionTypeEnum Type { get; set; } 
         
        public decimal Amount { get; set; }
 
        public DateTime Date { get; set; }  

         
        public string? Note { get; set; }
        public string? FromAccountName { get; set; }
        public string? ToAccountName { get; set; }
        public string? CategoryName { get; set; }
 
    }

}
