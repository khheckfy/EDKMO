using EDKMO.BusinessLogic.BusinessModels;
using EDKMO.BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDKMO.Web.Models
{
    public class EventSelectModel
    {
        public EventSelectModel(DateTime date)
        {
            Date = date.Date;
            Users = new List<UserDTO>();
            Hours = new List<TimeSpan>();
            Event = new EventDTO();
            Events = new List<EventDTO>();
            for (int i = 9; i < 18; i++)
            {
                Hours.Add(new TimeSpan(i, 0, 0));
                Hours.Add(new TimeSpan(i, 30, 0));
            }
        }

        public EventDTO Event { set; get; }

        /// <summary>
        /// Список событий на неделю
        /// </summary>
        public List<EventDTO> Events { set; get; }

        /// <summary>
        /// Выбранная дата
        /// </summary>
        public DateTime Date { private set; get; }
        /// <summary>
        /// Активные тренера
        /// </summary>
        public List<UserDTO> Users { set; get; }
        /// <summary>
        /// Текущая территория
        /// </summary>
        public TerritoryDTO Territory { set; get; }
        /// <summary>
        /// Часы клиента!
        /// </summary>
        public List<TimeSpan> Hours { private set; get; }

        public string Error { set; get; }
        public bool IsSuccess { set; get; }
    }
}