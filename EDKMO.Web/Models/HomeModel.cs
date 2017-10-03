using EDKMO.BusinessLogic.BusinessModels;
using EDKMO.BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDKMO.Web.Models
{
    public class HomeModel
    {
        public HomeModel()
        {
            Users = new List<UserDTO>();
        }

        /// <summary>
        /// Пользователи
        /// </summary>
        public List<UserDTO> Users { set; get; }

        public SchedulerDataObject SchedulerObject { set; get; }
    }
}