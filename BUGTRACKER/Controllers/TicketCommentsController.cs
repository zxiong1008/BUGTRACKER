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
    [Authorize]
    public class TicketCommentsController : Controller
    {
         ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketComments
        public ActionResult Index()
        {
            var ticketComments = db.TicketComments.Include(t => t.Author).Include(t => t.Ticket);
            return View(ticketComments.ToList());
        }

        // GET: TicketComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // GET: TicketComments/Create
        public ActionResult Create(int ticketId)
        {
            string userId = User.Identity.GetUserId();
            var ticket = db.Tickets.Find(ticketId);

            if (userId == ticket.AssignedUserId || userId == ticket.SubmitterId || userId == ticket.Project.ProjectManagerId || User.IsInRole("Admin"))
            {
                //ViewBag.TicketId = new SelectList(db.Tickets.Select(t => t.Id == id), "Id", "Title");
                //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");

                TicketComment ticketComment = new TicketComment { TicketId = ticketId };
            }

            return RedirectToAction("Details", "Ticket", new { id = ticketId });
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Body,Created, AuthorId")] int? ticketId, TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                ticketComment.AuthorId = User.Identity.GetUserId();
                ticketComment.Created = new DateTimeOffset(DateTime.Now);

                db.TicketComments.Add(ticketComment);
                db.SaveChanges();

                return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComment.AuthorId);
            return View(ticketComment);
        }

        // GET: TicketComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketComment.AuthorId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Body,Created,AuthorId,TicketId")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketComment.AuthorId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        // GET: TicketComments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userId = User.Identity.GetUserId();
            var ticket = db.Tickets.Find(id);

            if (userId == ticket.AssignedUserId || userId == ticket.SubmitterId || userId == ticket.Project.ProjectManagerId || User.IsInRole("Admin"))
            {
                TicketComment ticketComment = db.TicketComments.Find(id);
                if (ticketComment == null)
                {
                    return HttpNotFound();
                }
                return View(ticketComment);
            }
            return View(db.TicketComments.Find(id));
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComment ticketComment = db.TicketComments.Find(id);
            db.TicketComments.Remove(ticketComment);
            db.SaveChanges();
            //return RedirectToAction("Index");

            return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });

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
