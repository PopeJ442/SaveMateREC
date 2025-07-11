using Savemate.Application.Interface.IRepositories;
 
namespace Savemate.Infrastructure.Repository
{
    public class ApplicationUserRepository(SaveMateDbContext context) : BaseRepository<ApplicationUser>(context), IApplicationUserRepository
    {
    }
}
