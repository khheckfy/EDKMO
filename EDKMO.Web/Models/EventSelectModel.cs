using EDKMO.BusinessLogic.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDKMO.Web.Models
{
    public class EventSelectModel
    {
        public EventSelectModel()
        {
            DateTimes = new List<EventPlaningItem>();
        }

        public List<EventPlaningItem> DateTimes { set; get; }
        public List<TimeSpan> HeaderTimes
        {
            get
            {
                List<TimeSpan> list = new List<TimeSpan>();
                DateTimes.ForEach(date =>
                {
                    if (!list.Contains(date.Date.TimeOfDay))
                        list.Add(date.Date.TimeOfDay);
                });
                return list;
            }
        }

    }
}