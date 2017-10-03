using EDKMO.BusinessLogic.DTO;
using System.Collections;
using System.Collections.Generic;

namespace EDKMO.BusinessLogic.BusinessModels
{
    public class SchedulerDataObject
    {
        public IEnumerable Appointments { get; set; }
        public IEnumerable Resources { get; set; }
        public DevExpress.Web.Mvc.FetchAppointmentsMethod FetchAppointments { get; set; }
    }
}
