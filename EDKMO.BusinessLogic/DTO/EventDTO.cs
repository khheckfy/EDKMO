using System;

namespace EDKMO.BusinessLogic.DTO
{
    public class EventDTO
    {
        public int EventId { set; get; }

        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public DateTime ClientStartDate { set; get; }
        public DateTime ClientEndDate { set; get; }
        public DateTime CreatedOn { set; get; }

        public string RefEventName { set; get; }
        public string EventName { set; get; }
        public string ShortDescription { set; get; }
        public string LongDescription { set; get; }

        public Guid? AccountId { set; get; }
        public Guid? ReportMoId { set; get; }
        public bool IsMainEvent { set; get; }
        public byte UserId { set; get; }
        public byte EventTypeId { set; get; }
        public byte TerritoryId { set; get; }

        public TimeSpan EventDuration { set; get; }
        public TimeSpan RoadTo { set; get; }
        public TimeSpan RoadAfter { set; get; }
        public string UserName { set; get; }
        public string FaIcon { set; get; }
        public string EventTypeName { set; get; }
        public string EventColor { set; get; }
        public string ROUrl { set; get; }
        public int UTCHours { set; get; }
        public byte? RoId { set; get; }
        public string RoIdName { set; get; }
    }
}
