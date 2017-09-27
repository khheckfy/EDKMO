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

        public async Task<UserDTO> Get(byte id)
        {
            var data = await DB.UserRepository.FindByIdAsync(id);
            return Mapper.Map<User, UserDTO>(data);
        }

        public async Task Update(UserDTO model)
        {
            await DB.SaveChangesAsync();
        }

        public async Task Delete(byte id)
        {
            var obj = await DB.UserRepository.FindByIdAsync(id);
            obj.IsDisabled = true;
            await DB.SaveChangesAsync();
        }
    }
}
