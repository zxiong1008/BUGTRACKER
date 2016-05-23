using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BUGTRACKER.Models
{
    public class Project
    {
        public Project()
        {
            //key value pair - quickest to store
            //HashSet is not IEnumerable
            //when querying, IList is best
            this.Developers = new HashSet<ApplicationUser>();
            this.Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProjectManagerId { get; set; }

        //many to many relationship - table will be built automatically in SQL
        //such as ProjectApplicationUsers
        [InverseProperty("PMProjects")]
        public virtual ApplicationUser ProjectManager { get; set;}
        [InverseProperty("DevProjects")]
        public virtual ICollection<ApplicationUser> Developers { get; set;}
        public virtual ICollection<Ticket> Tickets { get; set; }
    }

    /********TableHistory and TableNotification will be used in the controller instead of the model*********/
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string Media { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        //ASPNETUSER table Id is a string so Assigned/Submitter will be string type
        public string AssignedUserId { get; set; }
        public string SubmitterId { get; set; }

        public int TicketStatusId { get; set; }
        public int ProjectId { get; set; }

        public int? TicketPriorityId { get; set; }
        public int? TicketTypeId { get; set; }

        public virtual TicketTypes TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        [AllowHtml]
        public virtual ICollection<TicketComment> Comments { get; set; }
        [AllowHtml]
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }

        public virtual Project Project { get; set; }

        [InverseProperty("TicketsAssigned")]
        public virtual ApplicationUser AssignedUser { get; set; }
        [InverseProperty("TicketsSubmitted")]
        public virtual ApplicationUser Submitter { get; set; }
    }

    public class TicketHistory
    {
        public int Id { get; set; }

        public int TicketId { get; set; }
        public string UserId { get; set; }

        //Id can change, so we want to keep the strings
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public string PropertyName { get; set; }
        public DateTimeOffset DateChanged { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

    public class TicketNotification
    {
        public int Id { get; set; }
        public int TargetUserId { get; set; }
        public DateTimeOffset DateSEnt { get; set; }
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }
    }

    public class TicketAttachment
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
    public class TicketComment
    {
        public int Id { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public string AuthorId { get; set; }
        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
    public class TicketPriority
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TicketStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TicketTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}