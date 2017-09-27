using EDKMO.Domain;
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

        public IQueryable Select()
        {
            return DB.TerritoryRepository.Query();
        }
    }
}
