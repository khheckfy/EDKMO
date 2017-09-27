using EDKMO.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKMO.BusinessLogic.Services
{
    public class UsersService : Interfaces.IUsers
    {
        IUnitOfWork DB;

        public UsersService(IUnitOfWork db)
        {
            DB = db;
        }

        public IQueryable Select()
        {
            return DB.UserRepository.Query();
        }
    }
}
