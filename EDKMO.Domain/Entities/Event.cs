using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDKMO.Domain.Entities
{
    [Table("Events")]
    public class Event
    {
        [Key]
        public int EventId { set; get; }

        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public DateTime CreatedOn { set; get; }

        [StringLength(512)]
        public string EventName { set; get; }
        [StringLength(512)]
        public string ShortDescription { set; get; }
        [StringLength(1024)]
        public string LongDescription { set; get; }

        public Guid? AccountId { set; get; }
        public Guid? ReportMoId { set; get; }

        public byte UserId { set; get; }
        public byte EventTypeId { set; get; }
        public byte TerritoryId { set; get; }

        public virtual User User { set; get; }
        public virtual EventType EventType { set; get; }
        public virtual Territory Territory { set; get; }

    }
}
