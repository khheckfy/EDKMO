using EDKMO.Domain.Entities;
using EDKMO.Domain.Repositories;

namespace EDKMO.Data.EntityFramework.Repositories
{
    internal class EventTypeRepository : Repository<EventType>, IEventTypeRepository
    {
        Model _context;

        internal EventTypeRepository(Model context)
            : base(context)
        {
            _context = context;
        }
    }
}
