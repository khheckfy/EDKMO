using AutoMapper;
using EDKMO.BusinessLogic.DTO;
using EDKMO.BusinessLogic.Interfaces;
using EDKMO.Domain;
using EDKMO.Domain.Entities;
using System.Collections.Generic;
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

        public List<EventTypeDTO> ListAllRazor()
        {
            var data = DB.EventTypeRepository.GetAll();
            return Mapper.Map<List<EventType>, List<EventTypeDTO>>(data);
        }

        public async Task<List<EventTypeDTO>> ListAll()
        {
            var data = await DB.EventTypeRepository.GetAllAsync();
            return Mapper.Map<List<EventType>, List<EventTypeDTO>>(data);
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
            obj.FaIcon = model.FaIcon;
            obj.IsRequiredReport = model.IsRequiredReport;

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
