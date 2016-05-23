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
using BLOG.Models;
using System.IO;
using System.Threading.Tasks;

namespace BUGTRACKER.Controllers
{
    [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper;
        private UserProjectsHelper projectHelper;
       
        public TicketsController()
        {
            this.projectHelper = new UserProjectsHelper(db);
            this.rolesHelper = new UserRolesHelper(db);
        }

        public ActionResult ViewAll()
        {
            List<Ticket> tickets = new List<Ticket>();
            tickets = db.Tickets.Include(t => t.AssignedUser).Include(t => t.Project).Include(t => t.Submitter).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType).ToList();

            return View(tickets);
        }

        // GET: Tickets
        public ActionResult Index()
        {
            List<Ticket> tickets = new List<Ticket>();
            
            //if user is ADMIN, show all tickets
            if (User.IsInRole("Admin"))
            {
                tickets = db.Tickets.Include(t => t.AssignedUser).Include(t => t.Project).Include(t => t.Submitter).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType).ToList();
            }
            //if user is PM, show ticket for all PM
            else if (User.IsInRole("ProjectManager"))
            {
                tickets = db.Users.Find(User.Identity.GetUserId()).PMProjects.SelectMany(p=>p.Tickets).OrderByDescending(t=>t.Created).ToList();
            }
            //if user is DEVELOPER, show all tickets assinged to
            else if (User.IsInRole("Developer"))
            {
                tickets = db.Users.Find(User.Identity.GetUserId()).TicketsAssigned.ToList();
            }
            //if user is SUBMITTER, show all tickets he has submitted
            else
            {
                tickets = db.Users.Find(User.Identity.GetUserId()).TicketsSubmitted.ToList();

            }
            return View(tickets.OrderByDescending(t => t.Created));
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticketId = db.Tickets.Find(id);
            if (ticketId == null)
            {
                return HttpNotFound();
            }
            return View(ticketId);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            //ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            //ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", db.TicketStatuses.FirstOrDefault(t => t.Name == "None").Id); //None
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", db.TicketStatuses.FirstOrDefault(t => t.Name == "New").Id); //New
            //ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", db.TicketTypes.FirstOrDefault(t => t.Name == "Unknown").Id); //Unknown
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Media,Created,Updated,AssignedUserId,SubmitterId,TicketStatusId,ProjectId,TicketPriorityId,TicketTypeId")] Ticket ticket, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                ticket.AssignedUserId = db.Projects.Find(ticket.ProjectId).ProjectManagerId;
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New").Id;
                ticket.TicketTypeId = db.TicketTypes.FirstOrDefault(t => t.Name == "Unknown").Id;
                ticket.TicketPriorityId = db.TicketPriorities.FirstOrDefault(t => t.Name == "None").Id;


                ticket.SubmitterId = User.Identity.GetUserId();
                ticket.Created = new DateTimeOffset(DateTime.Now);

                
                //apply and decide which to show upon role
                //on view, wrap form groups inside statements
                //admin show admin
                //pm show pm only
                //dev create ticket, show certain things.

                if (ImageValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/images/ticket/"), fileName));
                    ticket.Media = "~/images/ticket/" + fileName;
                }

                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedUserId = new SelectList(db.Users, "Id", "DisplayName", ticket.AssignedUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            //ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatus);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketType);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userId = User.Identity.GetUserId();
            var ticketId = db.Tickets.Find(id);

            if (userId == ticketId.AssignedUserId || userId == ticketId.SubmitterId || userId == ticketId.Project.ProjectManagerId || User.IsInRole("Admin"))
            {

                Ticket ticket = db.Tickets.Find(id);
                TempData["oldTicket"] = ticket;
                if (ticket == null)
                {
                    return HttpNotFound();
                }

                ViewBag.AssignedUserId = new SelectList(projectHelper.GetUsersOnProject(ticket.ProjectId), "Id", "DisplayName", ticket.AssignedUserId);
                //ViewBag.AssignedUserId = new SelectList(db.Project, "Id", "Name", ticket.AssignedUserId);
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
                //ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
                return View(ticket);
            }
            return View(db.Tickets.Find(id));

        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,Updated,AssignedUserId,SubmitterId,TicketStatusId,ProjectId,TicketPriorityId,TicketTypeId")] Ticket ticket, HttpPostedFileBase image )
        {
            if (ModelState.IsValid)
            {
                //OPTION 1:
                //find is similar to FirstOrDefault but its temporary storage.
                //FirstOrDefault() is a LINQ statement, much better than Find()
                //Ticket oldTicket = db.Tickets.Find(ticket.Id);

                //OPTION 2:
                //Query Statement
                //Works effectively if we dont use db.SaveChanges() in If statements
                //Ticket oldTicket = db.Tickets.FirstOrDefault(t => t.Id == ticket.Id);

                //OPTION 3:
                //Equivalent to the LINQ statement
                //creating a separate object of the memory.
                //This allows the user to access the old reference data,
                //the LINQ statement will access the new reference data, which CAN be incorrect
                //when using db.SaveChanges() in each If statement.
                Ticket oldTicket = (Ticket)TempData["oldTicket"];

                if (oldTicket.TicketPriorityId != ticket.TicketPriorityId)
                {
                    var history = new TicketHistory
                    {
                        DateChanged = new DateTimeOffset(DateTime.Now),
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        PropertyName = "Priority",
                        OldValue = oldTicket.TicketPriority.Name,
                        NewValue = db.TicketPriorities.Find(ticket.TicketPriorityId).Name
                    };
                    db.TicketHistories.Add(history);

                    //notify someone of these changes
                    await Notify(ticket.Id, history);
                }
                if (oldTicket.TicketTypeId != ticket.TicketTypeId)
                {
                    var history = new TicketHistory
                    {
                        DateChanged = new DateTimeOffset(DateTime.Now),
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        PropertyName = "Type",
                        OldValue = oldTicket.TicketType.Name,
                        NewValue = db.TicketTypes.Find(ticket.TicketTypeId).Name
                    };
                    db.TicketHistories.Add(history);
                    
                    //notify someone of these changes
                    await Notify(ticket.Id, history);

                }
                if (oldTicket.TicketStatusId != ticket.TicketStatusId)
                {
                    var history = new TicketHistory
                    {
                        DateChanged = new DateTimeOffset(DateTime.Now),
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        PropertyName = "Status",
                        OldValue = oldTicket.TicketStatus.Name,
                        NewValue = db.TicketStatuses.Find(ticket.TicketStatusId).Name
                    };
                    db.TicketHistories.Add(history);

                    //notify someone of these changes
                    await Notify(ticket.Id, history);

                }
                if (oldTicket.AssignedUserId != ticket.AssignedUserId)
                {
                    var history = new TicketHistory
                    {
                        DateChanged = new DateTimeOffset(DateTime.Now),
                        TicketId = ticket.Id,
                        UserId = User.Identity.GetUserId(),
                        PropertyName = "Assigned User",
                        OldValue = db.Users.Find(oldTicket.AssignedUser).DisplayName,
                        NewValue = db.Users.Find(ticket.AssignedUserId).DisplayName
                    };
                    db.TicketHistories.Add(history);

                    //notify someone of these changes
                    await Notify(ticket.Id, history);
                }

                ticket.Updated = new DateTimeOffset(DateTime.Now);
                db.Entry(ticket).State = EntityState.Modified;

                if (ImageValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/images/ticket/"), fileName));
                    ticket.Media = "~/images/ticket/" + fileName;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.AssignedUserId = new SelectList(projectHelper.GetUsersOnProject(ticket.ProjectId), "Id", "DisplayName", ticket.AssignedUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        public async Task Notify(int ticketId, TicketHistory history)
        {
            var db = new ApplicationDbContext();
            var ticket = db.Tickets.Include("Project").FirstOrDefault(t => t.Id == ticketId);
            var displayName = db.Users.Find(history.UserId).DisplayName;
            var eService = new EmailService();

            if (ticket.AssignedUserId == User.Identity.GetUserId())
            {
                //user is assigned user, send notification to PM

                await eService.SendAsync(new IdentityMessage
                {
                    Destination = "zxiong1008@gmail.com"/*ticket.Project.ProjectManager.Email*/,
                    Subject = "Developer Ticket Change",
                    Body = ticket.Project.ProjectManager.FirstName + ", <p/><b>" + displayName +
                           "</b> has changed the <b>'" + history.PropertyName + "'</b> of ticket <b>'" +
                           ticket.Title + "'</b> from <mark>" + history.OldValue + "</mark> to <mark>" + history.NewValue + "</mark>."
                });
            }
            else
            {
                //user is PM or Admin, send notification to AssignedUser
                await eService.SendAsync(new IdentityMessage
                {
                    Destination = "zxiong1008@gmail.com"/*ticket.AssignedUser.Email*/,
                    Subject = "PM or Admin Ticket Change",
                    Body = ticket.AssignedUser.FirstName + ", <p/><b>" + displayName +
                           "</b> has changed the <b>'" + history.PropertyName + "'</b> of ticket <b>'" +
                           ticket.Title + "'</b> from <mark>" + history.OldValue + "</mark> to <mark>" + history.NewValue + "</mark>."
                });
            }
        }


        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
