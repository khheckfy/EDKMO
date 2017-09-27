using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDKMO.Domain.Entities
{
    [Table("Users")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte UserId { set; get; }
        [StringLength(20)]
        public string DomainName { set; get; }
        [StringLength(50)]
        public string LastName { set; get; }
        [StringLength(50)]
        public string FirstName { set; get; }
        [StringLength(50)]
        public string MiddleName { set; get; }
        public TimeSpan StartWork { set; get; }
        public TimeSpan EndWork { set; get; }
        public bool IsDisabled { set; get; }

        public byte TerritoryId { set; get; }
        public virtual Territory Territory { set; get; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
