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

        public string FullName
        {
            get
            {
                string name = LastName;
                if (!string.IsNullOrEmpty(FirstName))
                    if (!string.IsNullOrEmpty(name))
                        name += string.Format(" {0}", FirstName);
                    else
                        name = FirstName;
                if (!string.IsNullOrEmpty(MiddleName))
                    if (!string.IsNullOrEmpty(name))
                        name += string.Format(" {0}", MiddleName);
                    else
                        name = MiddleName;
                return name;
            }
        }
    }
}
