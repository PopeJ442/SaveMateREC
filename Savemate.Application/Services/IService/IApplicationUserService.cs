 
using Savemate.Domain.Entities;
using Savemate.Infrastructure;


namespace Savemate.Application.Services.IService
{
    public interface IApplicationUserService 
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> AddUserAsync(ApplicationUser user);
        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);
        Task DeleteUserAsync(ApplicationUser user);
    }
}
