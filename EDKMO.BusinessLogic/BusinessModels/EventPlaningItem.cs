using System;

namespace EDKMO.BusinessLogic.BusinessModels
{
    public class EventPlaningItem
    {
        public EventPlaningItem(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { private set; get; }
        public byte UsertId { set; get; }
    }
}
