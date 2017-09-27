using System;
namespace EDKMO.BusinessLogic.DTO
{
    public class UserDTO
    {
        public byte UserId { set; get; }
        public string DomainName { set; get; }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public string MiddleName { set; get; }
        public TimeSpan StartWork { set; get; }
        public TimeSpan EndWork { set; get; }
        public bool IsDisabled { set; get; }
        public byte TerritoryId { set; get; }
    }
}
