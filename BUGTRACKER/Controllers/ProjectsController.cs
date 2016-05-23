using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BUGTRACKER.Models;

using Microsoft.AspNet.Identity;

namespace BUGTRACKER.Controllers
{
    [Authorize(Roles = "Admin, ProjectManager, Developer")]
    public class ProjectsController : Controller
    {
        //Create a private instance of the database context AND the roles helper class
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectsHelper projectsHelper;
        private UserRolesHelper rolesHelper;

        //purpose of constructor: initializes it
        public ProjectsController()
        {
            //instantiate the roles
            projectsHelper = new UserProjectsHelper(db);
            rolesHelper = new UserRolesHelper(db);
        }

        // GET: Projects
        public ActionResult Index()
        {
            IList<Project> projects;
            //Task #3: Restrict project views
            //How do we determine which projects to show?
            //If the user is an admin, he should see all projects
            //var projects = db.Projects.Include(p => p.ProjectManager);
            //eager loading -> .Include ^
            //lazy loading db.Projects.ToList();

            if(User.IsInRole("Admin"))
            {
                projects = db.Projects.ToList();
            }
            //If the user in a project manager, he should only see the project he manages
            else if (User.IsInRole("ProjectManager"))
            {
                projects = db.Users.Find(User.Identity.GetUserId()).PMProjects.ToList();
            }
            //If the user in a developer, he should only see the project he's assinged to
            else if(User.IsInRole("Developers"))
            {
                projects = db.Users.Find(User.Identity.GetUserId()).DevProjects.ToList();
            }
            else
            {
                return View(new List<Project>());
            }

            return View(projects);
        }

        //GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Create()
        {
            ViewBag.ProjectManagerId = new SelectList(rolesHelper.GetUsersInRole("ProjectManager"), "Id", "UserName");
            //ViewBag.ProjectManagerId = new SelectList(db.Users, "Id", "FirstName");

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Create([Bind(Include = "Id,Name,ProjectManagerId")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.ProjectManagerId = new SelectList(db.Users, "Id", "FirstName", project.ProjectManagerId);
            ViewBag.ProjectManagerId = new SelectList(rolesHelper.GetUsersInRole("ProjectManager"), "Id", "UserName", project.ProjectManagerId);

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Edit(int? id)
        {
            //if there is no id submitted, return a bad request status code
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find the selected role, if not found, return a not found response code
            var project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            //guild the view model for the selected role

            //var model = new RolesViewModel();
            //model.RoleId = role.Id;
            //model.RoleName = role.Name;

            //equivalent ^
            var model = new ProjectsViewModel { ProjectId = project.Id, ProjectName = project.Name };

            //get a list of all DEVELOPERS from the database
            model.Developers = new MultiSelectList(rolesHelper.GetUsersInRole("Developer"), "Id", "UserName");
            //get a list of DEVELOPER which are currently assinged to this project
            model.SelectedDevelopers = projectsHelper.GetUsersOnProject(project.Id).Select(u => u.Id).ToArray();
            
            //get a list of all PROJECT MANAGER from the database - select the item automatically from the dropdown box in details
            model.ProjectManager = new SelectList(rolesHelper.GetUsersInRole("ProjectManager"), "Id", "UserName"/*, project.ProjectManagerId*/);
            //get a list of PM which are currently assinged to this project
            model.SelectedProjectManager = project.ProjectManagerId;

            //model.SelectedProjectManager = projectsHelper.GetUsersOnProject(project.Id).ToString();


            //send the model to the view
            return View(model);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Edit([Bind(Include = "ProjectId, ProjectName, SelectedDevelopers, SelectedProjectManager")] ProjectsViewModel model)
        {
            //check the model state - if valid, proceed, otherwise return the model to the view
            if (ModelState.IsValid)
            {
                //find the role for the submitted model
                //var role = db.Roles.Find(id); -previous code, we will edit it using model.RoleId
                //var project = db.Projects.Find(model.ProjectId);

                var project = db.Projects.FirstOrDefault(p => p.Id == model.ProjectId);

                project.Name = model.ProjectName;
                project.ProjectManagerId = model.SelectedProjectManager;


                foreach (var user in project.Developers.ToList())
                {
                    if (!model.SelectedDevelopers.Contains(user.Id))
                    {
                        //remove the user if NOT in the new selected user array
                        projectsHelper.RemoveUserFromProject(user.Id, project.Id);
                    }
                }

                foreach (var userId in model.SelectedDevelopers)
                {
                    if (model.SelectedDevelopers.Contains(userId))
                    {
                        //adds the user if NOT in the new selected user array
                        projectsHelper.AddUserToProject(userId, project.Id);
                    }
                    else
                    {
                       Console.WriteLine("Select a developer.");
                        return RedirectToAction("Edit", "Projects", new { id = model.ProjectId });
                    }
                }

                //var currentDeveloper = projectsHelper.GetUsersOnProject(model.ProjectId);
                //var currentPM = projectsHelper.GetUsersOnProject(model.ProjectId);

                ////update the users in the role from the selected users in the model
                //foreach (var userId in currentDeveloper)
                //{
                //    //remove the user if NOT in the new selected users array
                //    if (!model.SelectedDevelopers.Contains(userId))
                //    {
                //        projectsHelper.RemoveUserFromProject(userId, model.ProjectId);
                //    }
                //}
                //foreach (var userId in model.SelectedDevelopers)
                //{
                //    //add a newly selected users
                //    if (!projectsHelper.IsUserInProject(userId, model.ProjectId))
                //    {
                //        projectsHelper.AddUserToProject(userId, model.ProjectId);
                //    }
                //}

                //////update the users in the role from the selected users in the model
                //foreach (var userId in currentPM)
                //{
                //    //remove the user if NOT in the new selected users array
                //    if (!model.SelectedProjectManager.Contains(userId))
                //    {
                //        projectsHelper.RemoveUserFromProject(userId, model.ProjectId);
                //    }
                //}

                ////add a newly selected users
                //if (!projectsHelper.IsUserInProject(model.SelectedProjectManager, model.ProjectId))
                //{
                //    projectsHelper.AddUserToProject(model.SelectedProjectManager, model.ProjectId);
                //}

                //naviage back to the roles index page of this controller
                return RedirectToAction("Index", "Projects", new { id = model.ProjectId});
            }
            return View(model);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
