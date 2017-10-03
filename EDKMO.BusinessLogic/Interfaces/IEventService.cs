using EDKMO.BusinessLogic.BusinessModels;
using EDKMO.BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDKMO.BusinessLogic.Interfaces
{
    public interface IEventService
    {
        Task<EventCreateResult> CreateEvent(EventDTO evnt);
        Task<List<EventDTO>> ListByDate(DateTime date, byte? territoryId = null, byte? userId = null);
        Task<SchedulerDataObject> ScheduleObject();
        Task Remove(int id);
    }
}
