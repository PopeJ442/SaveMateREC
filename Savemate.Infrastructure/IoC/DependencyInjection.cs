using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Services;
using Savemate.Application.Services.IService;
using Savemate.Infrastructure.CustomPolicy;
using Savemate.Infrastructure.Repository;
 


namespace Savemate.Infrastructure.IoC
{
    public class DependencyInjection
    {
        public static void AddingDbContext(IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<SaveMateDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        }
        public static void IdentityOption(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(opt => {
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
                opt.Password.RequiredLength = 8;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
               
            });
        }

        public static void Registering(IServiceCollection services ) 
        {
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IPasswordValidator<ApplicationUser>, CustomPasswordPolicy>();
            services.AddScoped<IUserValidator<ApplicationUser>, CustomUserEmailPolicy>();
        
        
        }
       
    }
}