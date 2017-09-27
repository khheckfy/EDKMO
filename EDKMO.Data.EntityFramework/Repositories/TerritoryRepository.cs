using EDKMO.Domain.Entities;
using EDKMO.Domain.Repositories;

namespace EDKMO.Data.EntityFramework.Repositories
{
    internal class TerritoryRepository : Repository<Territory>, ITerritoryRepository
    {
        Model _context;

        internal TerritoryRepository(Model context)
            : base(context)
        {
            _context = context;
        }
    }
}
