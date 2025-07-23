using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Infrastructure.CustomPolicy
{
    public class CustomPasswordPolicy :PasswordValidator<ApplicationUser>
    { 
        
        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string? password)
        {
            IdentityResult result = await base.ValidateAsync(manager, user, password);

            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>(): result.Errors.ToList();

            if (password.ToLower().Contains(user.UserName!.ToLower()))
                {
                errors.Add(new IdentityError
                {

                    Description = "Password cannot contain username"


                });      
            }
            if (password.Contains("123"))
            {
                errors.Add(new IdentityError
                {

                    Description = "Password cannot contain 123 numeric sequence"
                });
            
            }

            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());

        }

        

    }
}
