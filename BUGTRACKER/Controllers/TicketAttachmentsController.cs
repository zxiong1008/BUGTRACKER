﻿using System;
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

namespace BUGTRACKER.Controllers
{
    [Authorize]
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        
        // GET: TicketAttachments/Create
        public ActionResult Create(int ticketId)
        {
            string userId = User.Identity.GetUserId();
            var ticket = db.Tickets.Find(ticketId);

            if(userId == ticket.AssignedUserId || userId == ticket.SubmitterId || userId == ticket.Project.ProjectManagerId || User.IsInRole("Admin"))
            {
                ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
                return View();
            }

            return RedirectToAction("Details", "Ticket", new { id = ticketId });
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AuthorId,Description,Url,Created,TicketId")] TicketAttachment ticketAttachment, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                ticketAttachment.AuthorId = User.Identity.GetUserId();
                ticketAttachment.Created = new DateTimeOffset(DateTime.Now);

                if (ImageValidator.IsWebFriendlyImage(image))
                {
                    var fileAttachment = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/images/attachments/"), fileAttachment));
                    ticketAttachment.Url = "~/images/attachments/" + fileAttachment;
                }

                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });

            }

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);

            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);

            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AuthorId,Description,Url,Created,TicketId")] TicketAttachment ticketAttachment, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (ImageValidator.IsWebFriendlyImage(image))
                {
                    var fileAttachment = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/images/attachments/"), fileAttachment));
                    ticketAttachment.Url = "~/images/attachments/" + fileAttachment;
                }

                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
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
                TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
                if (ticketAttachment == null)
                {
                    return HttpNotFound();
                }
                return View(ticketAttachment);
            }
            return View(db.TicketAttachments.Find(id));
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Details", "Tickets", new { id = ticketAttachment.TicketId });

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
