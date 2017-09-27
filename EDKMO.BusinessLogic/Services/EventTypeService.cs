using AutoMapper;
using EDKMO.BusinessLogic.DTO;
using EDKMO.BusinessLogic.Interfaces;
using EDKMO.Domain;
using EDKMO.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace EDKMO.BusinessLogic.Services
{
    public class EventTypeService : IEventTypeService
    {
        IUnitOfWork DB;

        public EventTypeService(IUnitOfWork db)
        {
            DB = db;
        }

        public IQueryable Select()
        {
            return DB.EventTypeRepository.Query();
        }

        public async Task<EventTypeDTO> Get(byte id)
        {
            var data = await DB.EventTypeRepository.FindByIdAsync(id);
            return Mapper.Map<EventType, EventTypeDTO>(data);
        }

        public async Task Update(EventTypeDTO model)
        {
            EventType obj = new EventType();
            if (model.EventTypeId > 0)
                obj = await DB.EventTypeRepository.FindByIdAsync(model.EventTypeId);

            obj.Name = model.Name;
            obj.Color = model.Color;

            if (model.EventTypeId == 0)
                DB.EventTypeRepository.Add(obj);

            await DB.SaveChangesAsync();
        }

        public async Task Delete(byte id)
        {
            var obj = await DB.EventTypeRepository.FindByIdAsync(id);
            DB.EventTypeRepository.Remove(obj);
            await DB.SaveChangesAsync();
        }
    }
}
