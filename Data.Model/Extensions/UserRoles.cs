using Data.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Model.Extensions
{
    public static partial class EntityExtensions
    {
        public static void AddRoleToUser(this ApplicationContext cntx, AppUser user, Role role)
        {
            var lnk = cntx.UserRoles.FirstOrDefault(x => x.RoleId == role.RoleId && x.AppUserId == user.AppUserId);
            if (lnk == null)
            {
                cntx.UserRoles.Add(new UserRole { RoleId = role.RoleId, AppUserId = user.AppUserId });
            }
            else
            {
                lnk.IsArchived = false;
                cntx.UserRoles.Update(lnk);
            }
        }
        public static void AddRolesToUser(this ApplicationContext cntx, AppUser user, IEnumerable<Guid> roleIds)
        {
            foreach (var roleId in roleIds)
            {
                var lnk = cntx.UserRoles.FirstOrDefault(x => x.RoleId == roleId && x.AppUserId == user.AppUserId);
                if (lnk == null)
                {
                    cntx.UserRoles.Add(new UserRole { RoleId = roleId, AppUserId = user.AppUserId });
                }
                else
                {
                    lnk.IsArchived = false;
                    cntx.UserRoles.Update(lnk);
                }
            }
        }

        public static void RemoveRoleFromUser(this ApplicationContext cntx, AppUser user, Role role)
        {
            var lnk = cntx.UserRoles.FirstOrDefault(x => x.RoleId == role.RoleId && x.AppUserId == user.AppUserId);
            if (lnk != null)
            {
                lnk.IsArchived = true;
                cntx.UserRoles.Update(lnk);
            }
        }
        public static void RemoveAllRolesFromUser(this ApplicationContext cntx, AppUser user)
        {
            var lnk = cntx.UserRoles.FirstOrDefault(x => x.AppUserId == user.AppUserId);
            if (lnk != null)
            {
                lnk.IsArchived = true;
                cntx.UserRoles.Update(lnk);
            }
        }
    }
}
