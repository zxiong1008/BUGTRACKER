using BUGTRACKER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BUGTRACKER.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
        public ActionResult Dashboard()
        {
            DashboardViewModel model = new DashboardViewModel();

            //goes into a project db and takes the top 5 of a list and order it by desc of name.
            //and returns it to the projects model
            model.Projects = db.Projects.OrderByDescending(p => p.Name).Take(5).ToList();
            //goes into a ticket db and takes the top 10 of a list and order it by desc of name.
            //and returns it to the tickets model
            model.Tickets = db.Tickets.OrderByDescending(t => t.Updated).Take(10).ToList();

            ViewBag.Message = "The dashboard shows the most recent elements of the projects and tickets page. Click on dashboard/tickets tab to view new recent tickets.";
            //return the projects and tickets model to the view
            return View(model);
        }
        [Authorize]
        public ActionResult LoginNewUsername()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ListUserRole()
        {
            return View(db.Users.ToList());
        }
    }
}