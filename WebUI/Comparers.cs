using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Areas.Admin.Models;

namespace WebUI
{
    public class UsersComparer : IEqualityComparer<UserListVM>
    {
        public bool Equals([AllowNull] UserListVM x, [AllowNull] UserListVM y)
        {
            if (x == null || y == null) return false;
            return x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] UserListVM obj)
        {
            return 0;
        }
    }
}