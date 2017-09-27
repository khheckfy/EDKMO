using EDKMO.BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKMO.BusinessLogic.Interfaces
{
    public interface IEventTypeService
    {
        IQueryable Select();
        Task<EventTypeDTO> Get(byte id);
        Task Update(EventTypeDTO model);
        Task Delete(byte id);
    }
}
