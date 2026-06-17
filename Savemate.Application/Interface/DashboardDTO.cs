using Savemate.Application.ViewModels;

namespace Savemate.Application.Interface
{
    public class DashboardDTO
    {
        public decimal TotalBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }

        public List<TransactionDTO> RecentTransactions { get; set; } = new();
    }
}
