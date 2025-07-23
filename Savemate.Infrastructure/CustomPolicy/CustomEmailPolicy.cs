using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Infrastructure.CustomPolicy
{
    public class CustomUserEmailPolicy :UserValidator<ApplicationUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user) 
        {
            IdentityResult result =await base.ValidateAsync(manager, user); 

           List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList() ;
            if (user.UserName == "google")
            {
                errors.Add(new IdentityError
                {
                    Description = "Google cannot be used as a user name"
                });
            }
            if (!user.Email.ToLower().EndsWith("@gmail.com"))
            {
                errors.Add(new IdentityError {
                
                Description = "Only google mails are allowed"
                
                });
            }

            return errors.Count ==0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }

    }
}
