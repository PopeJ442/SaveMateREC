using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Savemate.Application.Common;
using Savemate.Application.Common.Extensions;
using Savemate.Application.Interface.IRepositories;
using Savemate.Application.Interfaces.Repositories;
using Savemate.Application.Interfaces.Services;
using Savemate.Application.Services;
using Savemate.Application.Services.IService;
using Savemate.Application.Services.IService.IAccountService;
using Savemate.Infrastructure.CustomPolicy;
using Savemate.Infrastructure.Repositories;
using Savemate.Infrastructure.Repository;


namespace Savemate.Infrastructure.IoC
{
    public class DependencyInjection
    {
        public static void AddingDbContext(IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<SaveMateDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


          //  services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        }
        public static void IdentityOption(IServiceCollection services)
        {
                services.Configure<IdentityOptions>(opt => {
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opt.Password.RequiredLength = 8;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
               
            });
        }

        public static void Registering(IServiceCollection services ) 
        {
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPasswordValidator<ApplicationUser>, CustomPasswordPolicy>();
            services.AddScoped<IUserValidator<ApplicationUser>, CustomUserEmailPolicy>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionService,TransactionService>();
            services.AddScoped<ITransactionAuditRepository, TransactionAuditRepository>();
            services.AddScoped<ITransactionAuditService, TransactionAuditService>();
            services.AddScoped<IDashboardService, DashboardService>(); 
            services.AddTransient<EmailHelper>();
 

        }



    }
}