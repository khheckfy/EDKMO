using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDKMO.Domain.Entities
{
    [Table("EventTypes")]
    public class EventType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte EventTypeId { set; get; }
        [StringLength(50)]
        public string Name { set; get; }
        [StringLength(128)]
        public string Color { set; get; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
