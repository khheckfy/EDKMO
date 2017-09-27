using EDKMO.BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKMO.BusinessLogic.Interfaces
{
    public interface IUsers
    {
        IQueryable Select();
        Task<UserDTO> Get(byte id);
        Task Update(UserDTO model);
        Task Delete(byte id);
    }
}
