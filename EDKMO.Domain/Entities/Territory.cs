using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDKMO.Domain.Entities
{
    [Table("Territories")]
    public class Territory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte TerritoryId { set; get; }
        [StringLength(5)]
        public string Name { set; get; }
        [StringLength(512)]
        public string ServerPath { set; get; }
        public byte UTCHours { set; get; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
