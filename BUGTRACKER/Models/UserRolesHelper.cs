using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BUGTRACKER.Models
{
    public class UserRolesHelper
    {
        //we need 1) a database context, 2) a UserManager object, and 3) a RoleManager object
        private ApplicationDbContext db;
        private ApplicationUserManager um;
        private RoleManager<IdentityRole> rm;

        public UserRolesHelper(ApplicationDbContext context)
        {
            this.db = context;
            this.um = new ApplicationUserManager(new UserStore<ApplicationUser>(this.db));
            this.rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(this.db));
        }

        //is user in role?
        public bool IsUserInRole(string userId, string roleName)
        {
            return um.IsInRole(userId, roleName);
        }

        //add user to role 
        public async System.Threading.Tasks.Task<bool> AddUserToRole(string userId, string roleName)
        {
            //var result = new IdentityResult();
            if(!um.IsInRole(userId, roleName))
            {
                var result = await um.AddToRoleAsync(userId, roleName);
                return result.Succeeded;
            }
            //return result.Succeeded
            return true;
        }

        //remove user from role
        public async System.Threading.Tasks.Task<bool> RemoveUserFromRole(string userId, string roleName)
        {
            if(um.IsInRole(userId, roleName))
            {
                var result = await um.RemoveFromRoleAsync(userId, roleName);
                return result.Succeeded;
            }
            return true;
        }

        //get user role
        public IList<string> GetUserRoles(string userId)
        {
            return um.GetRoles(userId);
        }

        //get users in role
        public IList<ApplicationUser> GetUsersInRole(string roleName)
        {
            var userIds = rm.FindByName(roleName).Users.Select(u => u.UserId).ToList();
            return db.Users.Where(u => userIds.Contains(u.Id)).ToList();
        }

        //get users in role
        public IList<ApplicationUser> GetUsersNotInRole(string roleName)
        {
            var userIds = rm.FindByName(roleName).Users.Select(u => u.UserId).ToList();
            return db.Users.Where(u => !userIds.Contains(u.Id)).ToList();
        }

        //get users not in any role
        public IList<ApplicationUser> GetUsersNotInAnyRole()
        {
            return um.Users.Where(u => u.Roles.Count == 0).ToList();
        }
    }
}