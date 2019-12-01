using EDKMO.BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EDKMO.Web.Models
{
    public class EventBlockFormModel
    {
        private readonly static TimeSpan StartTime = new TimeSpan(8, 0, 0);
        private readonly static TimeSpan EndTime = new TimeSpan(18, 0, 0);

        public EventBlockFormModel() { }

        public EventBlockFormModel(byte userId, string userName)
        {
            UserId = userId;
            UserName = userName;

            DateEnd =
            DateStart =
                DateTime.Now.Date;

            ClientTimeFrom =
                TimeFrom =
               StartTime;
            ClientTimeTo =
                TimeTo =
                EndTime;
        }

        public string UserName { private set; get; }
        public byte UserId { set; get; }

        public byte EventTypeId { set; get; }
        public string LongDesc { set; get; }
        public string ShortDesc { set; get; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateStart { set; get; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateEnd { set; get; }
        public TimeSpan TimeFrom { set; get; }
        public TimeSpan TimeTo { set; get; }

        public List<EventTypeDTO> EventTypes { set; get; }
        public List<TerritoryDTO> Ros { set; get; }

        public TimeSpan ClientTimeFrom { set; get; }
        public TimeSpan ClientTimeTo { set; get; }
        public byte RoId { set; get; }
    }
}