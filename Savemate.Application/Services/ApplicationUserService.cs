using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteUserAsync(ApplicationUser user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new Exception("User not found");

            await _userRepository.DeleteAsync(existingUser);
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
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null) throw new Exception("User not found");

            // Update the fields manually
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.MiddleName = user.MiddleName;
            existingUser.DOB = user.DOB;
            existingUser.Email = user.Email;
            existingUser.UserName = user.Email;

            await _userRepository.SaveChangesAsync(user);

            return existingUser;
        }


    }
}
