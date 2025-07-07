using Savemate.Application.Interface.IRepositories;
using Savemate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Infrastructure.Repository
{
    public class ApplicationUserRepository(SaveMateDbContext context) : BaseRepository<ApplicationUser>(context), IApplicationUserRepository
    {
    }
}
