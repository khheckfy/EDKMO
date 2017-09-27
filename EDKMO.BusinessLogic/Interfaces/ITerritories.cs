using EDKMO.BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKMO.BusinessLogic.Interfaces
{
    public interface ITerritories
    {
        IQueryable Select();
        Task<TerritoryDTO> Get(byte id);
        Task Update(TerritoryDTO model);
        Task<List<TerritoryDTO>> GetAll();
    }
}
