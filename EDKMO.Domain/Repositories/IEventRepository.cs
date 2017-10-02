using EDKMO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDKMO.Domain.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<List<Event>> ListByDate(DateTime date, byte? territoryId = null, byte? userId = null);
    }
}
