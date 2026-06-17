
using Savemate.Application.Interface;
 
namespace Savemate.Application.Services.IService
{
    public interface IDashboardService
    {

        Task<DashboardDTO> GetDashboardDataAsync(string userId);
    }
}
