using EDKMO.Domain.Entities;
using EDKMO.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System.Data.Entity;

namespace EDKMO.Data.EntityFramework.Repositories
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        Model _context;

        internal UserRepository(Model context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<User>> SelectActive()
        {
            return await
                Set
                .Where(n => n.IsDisabled == false)
                .OrderBy(n => n.LastName)
                .ToListAsync();

        }
    }
}
