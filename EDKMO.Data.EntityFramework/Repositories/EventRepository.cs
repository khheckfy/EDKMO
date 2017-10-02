using EDKMO.Domain.Entities;
using EDKMO.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System.Data.Entity;

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

        public async Task<List<Event>> ListByDate(DateTime date, byte? territoryId = null, byte? userId = null)
        {
            DateTime dfrom = date.Date;
            DateTime dto = date.Date.AddDays(1).AddSeconds(-1);

            var query = from e in Set
                        where
                        e.StartDate >= dfrom && e.StartDate <= dto
                        select e;

            if (territoryId.HasValue)
                query = query.Where(n => n.TerritoryId == territoryId);
            if (userId.HasValue)
                query = query.Where(n => n.UserId == userId);

            return await query.ToListAsync();
        }
    }
}
