using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Services.IService;
using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

      
        public async Task<User> AddUserAsync(User user)
        {
              await _userRepository.AddAsync(user);
              return user;
        }
         
        public async Task  DeleteUserAsync(User user )
        {
            await _userRepository.DeleteAsync(user);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
              return  await _userRepository.GetAllAsync();

        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
              return   await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
              await _userRepository.UpdateAsync(user);
              return user;
        }
    }
}
