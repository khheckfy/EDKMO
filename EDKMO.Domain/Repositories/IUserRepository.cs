using EDKMO.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDKMO.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> SelectActive();
    }
}
