using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Model
{
    public enum RoleType
    {
        Undefined,
        User = 1,
        Manager = 200,
        Superuser = 500,
        Admin = 999
    }
    public enum Status
    {
        Active,
        Pending,
        InActive,
        Blocked
    }
}
