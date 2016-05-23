using BUGTRACKER.Models; //ApplicationDbConext

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BUGTRACKER.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        //Create a private instance of the database context AND the roles helper class
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper helper;

        //roles controller constructor which will initialize the helper class and pass the instance of the context we declare previously
        public RolesController()
        {
            //instantiate the roles
            helper = new UserRolesHelper(db);
        }

        // GET: Roles
        public ActionResult Index()
        {
            //get a list of roles from the db
            var roles = db.Roles.ToList();

            //create a view model for the Roles index page and select into it just the role id aor role names
            var model = new List<RolesViewModel>();

            //effective for large quantity of roles, using LINQ in a single statement
            model = roles.Select(r => new RolesViewModel { RoleId = r.Id, RoleName = r.Name }).ToList();
            
            ////effective for small quantity of roles
            //foreach (var r in roles)
            //{
            //    model.Add(new RolesViewModel { RoleId = r.Id, RoleName = r.Name });
            //}
            return View(model);
        }

        //we dont need a delete/details page for roles
        //getting role information and sending it to the view
        //next, we want to get a return and process it
        //and compare it.
        public ActionResult Edit(string id)
        {
            //if there is no id submitted, return a bad request status code
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find the selected role, if not found, return a not found response code
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            //guild the view model for the selected role
            
            //var model = new RolesViewModel();
            //model.RoleId = role.Id;
            //model.RoleName = role.Name;

            //equivalent ^
            var model = new RolesViewModel { RoleId = role.Id, RoleName = role.Name };

            //get a list of all user from the database
            model.User = new MultiSelectList(db.Users, "Id", "UserName");

            //get all user and the roles which they are assinged too
            model.SelectedUsers = helper.GetUsersInRole(role.Name).Select(u => u.Id).ToArray();

            //send the model to the view
            return View(model);
        }


        //this will post using the httpPost -- POINTING OUT THE OBVIOUS! DUH~ ZENG!
        [HttpPost]
        [ValidateAntiForgeryToken]
        //with the role manage, we dont want to work too fast
        public async System.Threading.Tasks.Task<ActionResult> Edit(RolesViewModel model)
        {
            //check the model state - if valid, proceed, otherwise return the model to the view
            if (ModelState.IsValid)
            {
                //find the role for the submitted model
                //var role = db.Roles.Find(id); -previous code, we will edit it using model.RoleId
                var role = db.Roles.Find(model.RoleId);
                var currentUsers = helper.GetUsersInRole(model.RoleName).Select(u => u.Id);

                //update the users in the role from the selected users in the model
                foreach (var userId in currentUsers)
                {
                    //remove the user if NOT in the new selected users array
                    if(!model.SelectedUsers.Contains(userId))
                    {
                        await helper.RemoveUserFromRole(userId, model.RoleName);
                    }
                }
                foreach(var userId in model.SelectedUsers)
                {
                    //add a newly selected users
                    if(!helper.IsUserInRole(userId, model.RoleName))
                    {
                        await helper.AddUserToRole(userId, model.RoleName);
                    }
                }
                //naviage back to the roles index page of this controller
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}