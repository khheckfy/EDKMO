using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKMO.BusinessLogic.BusinessModels
{
    public class EventPlaningItem
    {
        public EventPlaningItem(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { private set; get; }
        public bool IsFree { set; get; }

    }
}
