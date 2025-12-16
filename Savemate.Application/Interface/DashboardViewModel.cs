using Savemate.Application.ViewModels;

namespace Savemate.Application.Interface
{
    public class DashboardViewModel
    {
        public decimal TotalBalance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }

        public List<TransactionViewModel> RecentTransactions { get; set; } = new();
    }
}
