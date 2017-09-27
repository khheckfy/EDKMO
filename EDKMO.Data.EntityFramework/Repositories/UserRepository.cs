using EDKMO.Domain.Entities;
using EDKMO.Domain.Repositories;

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
    }
}
