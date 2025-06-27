using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
 

namespace Savemate.Infrastructure.IoC
{
    public class DependencyInjection
    {
        public static void AddingDbContext(IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<SaveMateDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")

           //     b => b.MigrationsAssembly("Savemate.Web")

                )



            );
        
        }
    }
}