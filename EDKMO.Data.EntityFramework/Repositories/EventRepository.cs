using EDKMO.Domain.Entities;
using EDKMO.Domain.Repositories;

namespace EDKMO.Data.EntityFramework.Repositories
{
    internal class EventRepository : Repository<Event>, IEventRepository
    {
        Model _context;

        internal EventRepository(Model context)
            : base(context)
        {
            _context = context;
        }
    }
}
