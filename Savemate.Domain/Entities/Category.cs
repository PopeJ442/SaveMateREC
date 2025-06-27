using Savemate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savemate.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public CategoryTypeEnum Type { get; set; }

        public User User { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = [];
    }
}
