using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Domain.Enums
{
    public enum AuditTypeEnum
    {
        Created = 1,
        Updated = 2,
        Reversed = 3,
        Deleted = 4
    }
}
