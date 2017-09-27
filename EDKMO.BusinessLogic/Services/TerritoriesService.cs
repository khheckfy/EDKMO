using AutoMapper;
using EDKMO.BusinessLogic.DTO;
using EDKMO.Domain;
using EDKMO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKMO.BusinessLogic.Services
{
    public class TerritoriesService : Interfaces.ITerritories
    {
        IUnitOfWork DB;

        public TerritoriesService(IUnitOfWork db)
        {
            DB = db;
        }

        public async Task<List<TerritoryDTO>> GetAll()
        {
            var data = await DB.TerritoryRepository.GetAllAsync();
            return Mapper.Map<List<Territory>, List<TerritoryDTO>>(data);
        }

        public IQueryable Select()
        {
            return DB.TerritoryRepository.Query();
        }

        public async Task<TerritoryDTO> Get(byte id)
        {
            var data = await DB.TerritoryRepository.FindByIdAsync(id);
            return Mapper.Map<Territory, TerritoryDTO>(data);
        }

        public async Task Update(TerritoryDTO model)
        {
            Territory obj = new Territory();
            if (model.TerritoryId > 0)
                obj = await DB.TerritoryRepository.FindByIdAsync(model.TerritoryId);

            obj.Name = model.Name;
            obj.ServerPath = model.ServerPath;
            obj.UTCHours = model.UTCHours;

            if (model.TerritoryId == 0)
                DB.TerritoryRepository.Add(obj);

            await DB.SaveChangesAsync();
        }
    }
}
