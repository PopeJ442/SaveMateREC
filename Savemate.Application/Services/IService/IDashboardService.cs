
using Savemate.Application.Interface;
 
namespace Savemate.Application.Services.IService
{
    public interface IDashboardService
    {

        Task<DashboardViewModel> GetDashboardDataAsync(string userId);
    }
}
