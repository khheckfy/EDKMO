using EDKMO.BusinessLogic.BusinessModels;
using EDKMO.BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDKMO.BusinessLogic.Interfaces
{
    public interface IEventService
    {
        Task UpdateEvent(EventDTO evnt);
        EventDTO Get(object id);
        Task<EventCreateResult> CreateEvent(EventDTO evnt);
        Task<List<EventDTO>> ListByDate(DateTime date, byte? territoryId = null, byte? userId = null);
        Task<SchedulerDataObject> ScheduleObject(List<byte> resources);
        Task Remove(int id);
        Task<EventCreateResult> CreateBlockEvent(EventDTO evnt);
    }
}
