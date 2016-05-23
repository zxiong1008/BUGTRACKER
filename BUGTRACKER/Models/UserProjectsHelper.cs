using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BUGTRACKER.Models
{
    public class UserProjectsHelper
    {
        //we need 1) a database context, 2) a UserManager object, and 3) a ProjectManager object
        private ApplicationDbContext db;
        private ApplicationUserManager um;
        private RoleManager<IdentityRole> rm;
        private UserRolesHelper rHelper;

        public UserProjectsHelper(ApplicationDbContext context)
        {
            this.db = context;
            this.rHelper = new UserRolesHelper(this.db);
            this.um = new ApplicationUserManager(new UserStore<ApplicationUser>(this.db));
            this.rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(this.db));
        }

        //is user in Project?
        public bool IsUserInProject(string userId, int projectId)
        {
            return db.Projects.Find(projectId).Developers.Any(u => u.Id == userId);
        }

        //add user to Project
        public bool AddUserToProject(string userId, int projectId)
        {
            if(!IsUserInProject(userId, projectId))
            {
                var user = um.FindById(userId);
                db.Projects.Find(projectId).Developers.Add(user);
                db.SaveChanges();
            }
            return IsUserInProject(userId, projectId);
        }

        //remove user from Project
        public bool RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserInProject(userId, projectId))
            {
                var user = um.FindById(userId);
                db.Projects.Find(projectId).Developers.Remove(user);
                db.SaveChanges();
            }
            return !IsUserInProject(userId, projectId);

        }

        //get user Project
        public IList<int> GetUserProjects(string userId)
        {
            return um.FindById(userId).DevProjects.Select(p => p.Id).ToList();
        }

        //get users in Project
        public IList<ApplicationUser> GetUsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Developers.ToList();
        }

        //get users in Project
        public IList<ApplicationUser> GetUsersNotOnProject(int projectId)
        {
            var usersOnProject = GetUsersOnProject(projectId).Select(u => u.Id);
            //var developers = rm.FindByName("Developer").Users.ToList();

            return rHelper.GetUsersInRole("Developer").Where(d => !usersOnProject.Contains(d.Id)).ToList();
        }

        //get users not in any Project
        public IList<ApplicationUser> GetUsersNotOnAnyProject()
        {
            var devRoleId = rm.FindByName("Developer").Id;
            var developers = um.Users.Where(u => u.Roles.Any(r => r.RoleId == devRoleId));
            return developers.Where(u => u.DevProjects.Count == 0).ToList();
        }
    }
}