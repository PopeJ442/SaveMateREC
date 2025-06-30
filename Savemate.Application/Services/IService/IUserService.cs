using Savemate.Application.Interface.IRepositories;
using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Services.IService
{
    public interface IUserService 
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User>GetUserByIdAsync(Guid id);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
