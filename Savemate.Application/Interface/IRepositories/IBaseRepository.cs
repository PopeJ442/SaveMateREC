using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Application.Interface.IRepositories
{
    public interface IBaseRepository <T>  where T : class
    {
        Task<T> AddAsync(object T);
        Task<T> GetByIdAsync(object id);

        Task<IEnumerable<T>> GetAllAsync();
        
        Task<T> UpdateAsync(object T);
        Task DeleteAsync(object id);
        
    }
}
