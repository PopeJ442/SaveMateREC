using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;
using Savemate.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Services
{
    public class ApplicationUserService(IApplicationUserRepository userRepository) : IApplicationUserService
    {
        private readonly IApplicationUserRepository _userRepository = userRepository;

      
        public async Task<ApplicationUser> AddUserAsync(ApplicationUser user)
        {
              await _userRepository.AddAsync(user);
              return user;
        }
         
        public async Task  DeleteUserAsync(ApplicationUser user )
        {
            await _userRepository.DeleteAsync(user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
              return  await _userRepository.GetAllAsync();

        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
              return   await _userRepository.GetByIdAsync(id);
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
              await _userRepository.UpdateAsync(user);
              return user;
        }
 
    }
}
