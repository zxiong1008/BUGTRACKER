using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BUGTRACKER.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; }

        public IList<string> UserRoles()
        {
            var helper = new UserRolesHelper(new ApplicationDbContext());
            return helper.GetUserRoles(this.Id);
        }

        [InverseProperty("Developers")]
        public virtual ICollection<Project> DevProjects { get; set; }
        [InverseProperty("ProjectManager")]
        public virtual ICollection<Project> PMProjects { get; set; }
        [InverseProperty("AssignedUser")]
        public virtual ICollection<Ticket> TicketsAssigned { get; set; }
        [InverseProperty("Submitter")]
        public virtual ICollection<Ticket> TicketsSubmitted { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketComment> TicketComments { get; set; }
        public virtual DbSet<TicketAttachment> TicketAttachments { get; set; }
        public virtual DbSet<TicketPriority> TicketPriorities { get; set; }
        public virtual DbSet<TicketStatus> TicketStatuses { get; set; }
        public virtual DbSet<TicketTypes> TicketTypes { get; set; }
        public virtual DbSet<TicketHistory> TicketHistories { get; set; }
        public virtual DbSet<TicketNotification> TicketNotifications { get; set; }

        public virtual DbSet<DemoLogin> DemoLogins { get; set; }
        public virtual DbSet<SendGridCredential> SendGridCredentials { get; set; }
    }

    public class DemoLogin
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        //public string Email { get; set; }
        public string Password { get; set; }
    }
    public class SendGridCredential
    {
        [Key]//setting up a 1-1 relationship using UserId as primarykey & foreignkey
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual ApplicationUser User { get; set; }
    }

}