using System;

namespace Data.Model
{
    public class CurrentUser
    {
        public CurrentUser()
        {
        }

        public ApplicationContext DbContext { get; set; }
        public Guid UserId { get; set; }
        public RoleType Role { get; set; }
        public int? StoreId { get; set; }
        public int? CityId { get; set; }
        public int? CompanyId { get; set; }
    }
}
