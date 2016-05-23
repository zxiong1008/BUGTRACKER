namespace BUGTRACKER.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BUGTRACKER.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BUGTRACKER.Models.ApplicationDbContext context)
        {
            /*******************Task 1: Create (4) roles : Admin, PM, Dev, and Sub*******************/
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            context.SaveChanges();

            /*******************Task 2: Make Admin, PM, Dev, and Submitter*******************/
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //Adding myself to the database
            if (!context.Users.Any(u => u.Email == "zxiong1008@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "zxiong1008@gmail.com",
                    UserName = "zxiong1008@gmail.com",
                    FirstName = "Zeng",
                    LastName = "Xiong",
                    DisplayName = "ZengC",
                    EmailConfirmed = true
                }, "Password-1");
            }
            //Adding Administrator1
            if (!context.Users.Any(u => u.Email == "Admin_Bugtracker1@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Admin_Bugtracker1@bt.com",
                    UserName = "Admin_Bugtracker1@bt.com",
                    FirstName = "Admin_1",
                    LastName = "Bugtracker1",
                    DisplayName = "Admin_1",
                    EmailConfirmed = true
                }, "Password-1");
            }

            //Adding Administrator2
            if (!context.Users.Any(u => u.Email == "Admin_Bugtracker2@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Admin_Bugtracker2@bt.com",
                    UserName = "Admin_Bugtracker2@bt.com",
                    FirstName = "Admin_2",
                    LastName = "Bugtracker2",
                    DisplayName = "Admin_2",
                    EmailConfirmed = true
                }, "Password-1");
            }

            //Adding Project Manager
            if (!context.Users.Any(u => u.Email == "PM_Bugtracker1@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "PM_Bugtracker1@bt.com",
                    UserName = "PM_Bugtracker1@bt.com",
                    FirstName = "Project_1",
                    LastName = "Manager1",
                    DisplayName = "PM_1",
                    EmailConfirmed = true
                }, "Password-1");
            }
            //Adding Project Manager
            if (!context.Users.Any(u => u.Email == "PM_Bugtracker2@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "PM_Bugtracker2@bt.com",
                    UserName = "PM_Bugtracker2@bt.com",
                    FirstName = "Project_2",
                    LastName = "Manager2",
                    DisplayName = "PM_2",
                    EmailConfirmed = true
                }, "Password-1");
            }

            //Adding Developer1
            if (!context.Users.Any(u => u.Email == "Dev_Bugtracker1@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Dev_Bugtracker1@bt.com",
                    UserName = "Dev_Bugtracker1@bt.com",
                    FirstName = "Dev_1",
                    LastName = "Bugtracker_",
                    DisplayName = "Developer_1",
                    EmailConfirmed = true
                }, "Password-1");
            }

            //Adding Developer2
            if (!context.Users.Any(u => u.Email == "Dev_Bugtracker2@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Dev_Bugtracker2@bt.com",
                    UserName = "Dev_Bugtracker2@bt.com",
                    FirstName = "Dev_2",
                    LastName = "Bugtracker",
                    DisplayName = "Developer_2",
                    EmailConfirmed = true
                }, "Password-1");
            }

            //Adding Developer3
            if (!context.Users.Any(u => u.Email == "Dev_Bugtracker3@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Dev_Bugtracker3@bt.com",
                    UserName = "Dev_Bugtracker3@bt.com",
                    FirstName = "Dev_3",
                    LastName = "Bugtracker3",
                    DisplayName = "Developer_3",
                    EmailConfirmed = true
                }, "Password-1");
            }

            //Adding Developer4
            if (!context.Users.Any(u => u.Email == "Dev_Bugtracker4@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Dev_Bugtracker4@bt.com",
                    UserName = "Dev_Bugtracker4@bt.com",
                    FirstName = "Dev_4",
                    LastName = "Bugtracker4",
                    DisplayName = "Developer_4",
                    EmailConfirmed = true
                }, "Password-1");
            }

            //Adding Submitter1
            if (!context.Users.Any(u => u.Email == "Submitter_Bugtracker1@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Submitter_Bugtracker1@bt.com",
                    UserName = "Submitter_Bugtracker1@bt.com",
                    FirstName = "Submitter_1",
                    LastName = "Bugtracker1",
                    DisplayName = "Submitter_1",
                    EmailConfirmed = true
                }, "Password-1");
            }
            
            //Adding Submitter2
            if (!context.Users.Any(u => u.Email == "Submitter_Bugtracker2@bt.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    Email = "Submitter_Bugtracker2@bt.com",
                    UserName = "Submitter_Bugtracker2@bt.com",
                    FirstName = "Submitter_2",
                    LastName = "Bugtracker2",
                    DisplayName = "Submitter_2",
                    EmailConfirmed = true
                }, "Password-1");
            }
            context.SaveChanges();

            /*******************Task 3: Add PM, Developers, and Submitter*******************/
            //admin
            var userId = userManager.FindByEmail("zxiong1008@gmail.com").Id;
            if (!userManager.IsInRole(userId, "Admin"))
            {
                userManager.AddToRole(userId, "Admin");
            }

            //Demo Admin 1
            userId = userManager.FindByEmail("Admin_Bugtracker1@bt.com").Id;
            if (!userManager.IsInRole(userId, "Admin"))
            {
                userManager.AddToRole(userId, "Admin");
            }

            //Demo Admin 2
            userId = userManager.FindByEmail("Admin_Bugtracker2@bt.com").Id;
            if (!userManager.IsInRole(userId, "Admin"))
            {
                userManager.AddToRole(userId, "Admin");
            }

            //Demo Project Manager 1
            userId = userManager.FindByEmail("PM_Bugtracker1@bt.com").Id;
            if (!userManager.IsInRole(userId, "ProjectManager"))
            {
                userManager.AddToRole(userId, "ProjectManager");
            }

            //Demo Project Manager 2
            userId = userManager.FindByEmail("PM_Bugtracker2@bt.com").Id;
            if (!userManager.IsInRole(userId, "ProjectManager"))
            {
                userManager.AddToRole(userId, "ProjectManager");
            }

            //Demo Developer 1
            userId = userManager.FindByEmail("Dev_Bugtracker1@bt.com").Id;
            if (!userManager.IsInRole(userId, "Developer"))
            {
                userManager.AddToRole(userId, "Developer");
            }

            //Demo Developer 2
            userId = userManager.FindByEmail("Dev_Bugtracker2@bt.com").Id;
            if (!userManager.IsInRole(userId, "Developer"))
            {
                userManager.AddToRole(userId, "Developer");
            }

            //Demo Developer 3
            userId = userManager.FindByEmail("Dev_Bugtracker3@bt.com").Id;
            if (!userManager.IsInRole(userId, "Developer"))
            {
                userManager.AddToRole(userId, "Developer");
            }

            //Demo Developer 4
            userId = userManager.FindByEmail("Dev_Bugtracker4@bt.com").Id;
            if (!userManager.IsInRole(userId, "Developer"))
            {
                userManager.AddToRole(userId, "Developer");
            }

            //Demo Submitter 1
            userId = userManager.FindByEmail("Submitter_Bugtracker1@bt.com").Id;
            if (!userManager.IsInRole(userId, "Submitter"))
            {
                userManager.AddToRole(userId, "Submitter");
            }

            //Demo Submitter 2
            userId = userManager.FindByEmail("Submitter_Bugtracker2@bt.com").Id;
            if (!userManager.IsInRole(userId, "Submitter"))
            {
                userManager.AddToRole(userId, "Submitter");
            }
            context.SaveChanges();
            /*******************Task 4: Ticket Priority*******************/
            if (!context.TicketPriorities.Any(t => t.Name == "Urgent"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Urgent" });
            }
            if (!context.TicketPriorities.Any(t => t.Name == "High"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "High" });
            }
            if (!context.TicketPriorities.Any(t => t.Name == "Medium"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Medium" });
            }
            if (!context.TicketPriorities.Any(t => t.Name == "Low"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "Low" });
            }
            if (!context.TicketPriorities.Any(t => t.Name == "None"))
            {
                context.TicketPriorities.Add(new TicketPriority { Name = "None" });
            }
            context.SaveChanges();

            /*******************Task 5: Ticket Status*******************/
            if (!context.TicketStatuses.Any(t => t.Name == "New"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "New" });
            }
            if (!context.TicketStatuses.Any(t => t.Name == "Working"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Working" });
            }
            if (!context.TicketStatuses.Any(t => t.Name == "QA"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "QA" });
            }
            //completed by Developer
            if (!context.TicketStatuses.Any(t => t.Name == "Completed"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Completed" });
            }
            //closed by PM
            if (!context.TicketStatuses.Any(t => t.Name == "Closed"))
            {
                context.TicketStatuses.Add(new TicketStatus { Name = "Closed" });
            }
            context.SaveChanges();

            /*******************Task 6:Ticket types*******************/
            if (!context.TicketTypes.Any(t => t.Name == "Bad Code"))
            {
                context.TicketTypes.Add(new TicketTypes { Name = "Bad Code" });
            }
            if (!context.TicketTypes.Any(t => t.Name == "Enhancements"))
            {
                context.TicketTypes.Add(new TicketTypes { Name = "Enhancements" });
            }
            if (!context.TicketTypes.Any(t => t.Name == "Design Features"))
            {
                context.TicketTypes.Add(new TicketTypes { Name = "Design Features" });
            }
            if (!context.TicketTypes.Any(t => t.Name == "Unknown"))
            {
                context.TicketTypes.Add(new TicketTypes { Name = "Unknown" });
            }
            context.SaveChanges();

            /*******************Task 7: Seed demo project*******************/
            var userId1 = userManager.FindByEmail("PM_Bugtracker1@bt.com").Id;
            var userId2 = userManager.FindByEmail("PM_Bugtracker2@bt.com").Id;
            if (!context.Projects.Any(t => t.Name == "Demo Project 1"))
            {
                context.Projects.Add(new Project
                {
                    Name = "Demo Project 1",
                    ProjectManagerId = userId1
                });
            }
            if (!context.Projects.Any(t => t.Name == "Demo Project 2"))
            {
                context.Projects.Add(new Project
                {
                    Name = "Demo Project 2",
                    ProjectManagerId = userId1
                });
            }

            if (!context.Projects.Any(t => t.Name == "Demo Project 3"))
            {
                context.Projects.Add(new Project
                {
                    Name = "Demo Project 3",
                    ProjectManagerId = userId2
                });
            }
            if (!context.Projects.Any(t => t.Name == "Demo Project 4"))
            {
                context.Projects.Add(new Project
                {
                    Name = "Demo Project 4",
                    ProjectManagerId = userId2
                });
            }
            //after creating the properties for each class, we will have to save it for the db to recongize them before moving further
            context.SaveChanges();

            /*******************Task 8: Seed a few demo tickets*******************/
            //seed ticket 1 project id #1
            var projectId1 = context.Projects.First(p => p.Name == "Demo Project 1").Id;
            var projectId2 = context.Projects.First(p => p.Name == "Demo Project 2").Id;
            var projectId3 = context.Projects.First(p => p.Name == "Demo Project 3").Id;
            var projectId4 = context.Projects.First(p => p.Name == "Demo Project 4").Id;

            //submitter id
            var submitterId1 = context.Users.First(p => p.Email == "Submitter_Bugtracker1@bt.com").Id;
            var submitterId2 = context.Users.First(p => p.Email == "Submitter_Bugtracker2@bt.com").Id;
            
            //ticket status id
            var ticketStatusIdN = context.TicketStatuses.First(p => p.Name == "New").Id;
            var ticketStatusIdW = context.TicketStatuses.First(p => p.Name == "Working").Id;
            var ticketStatusIdQ = context.TicketStatuses.First(p => p.Name == "QA").Id;
            var ticketStatusIdCp = context.TicketStatuses.First(p => p.Name == "Completed").Id;
            var ticketStatusIdCl = context.TicketStatuses.First(p => p.Name == "Closed").Id;

            //ticket type
            var ticketTypeIdB = context.TicketTypes.First(p => p.Name == "Bad Code").Id;
            var ticketTypeIdE = context.TicketTypes.First(p => p.Name == "Enhancements").Id;
            var ticketTypeIdD = context.TicketTypes.First(p => p.Name == "Design Features").Id;
            var ticketTypeIdU = context.TicketTypes.First(p => p.Name == "Unknown").Id;

            ////ticket priority
            var ticketPriorityIdU = context.TicketPriorities.First(p => p.Name == "Urgent").Id;
            var ticketPriorityIdH = context.TicketPriorities.First(p => p.Name == "High").Id;
            var ticketPriorityIdM = context.TicketPriorities.First(p => p.Name == "Medium").Id;
            var ticketPriorityIdL = context.TicketPriorities.First(p => p.Name == "Low").Id;
            var ticketPriorityIdN = context.TicketPriorities.First(p => p.Name == "None").Id;

            var assignedId1 = context.Users.First(p => p.UserName == "Dev_Bugtracker1@bt.com").Id;
            var assignedId2 = context.Users.First(p => p.UserName == "Dev_Bugtracker2@bt.com").Id;
            var assignedId3 = context.Users.First(p => p.UserName == "Dev_Bugtracker3@bt.com").Id;
            var assignedId4 = context.Users.First(p => p.UserName == "Dev_Bugtracker4@bt.com").Id;

            //************************BAD CODE TICKETS************************
            //**************************PROJECT #1****************************

            //ticket seed 1
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 1, P1"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 1, P1",
                    Description = "This is the first ticket for Demo project 1",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId1,
                    SubmitterId = submitterId1,
                    TicketStatusId = ticketStatusIdN,
                    TicketTypeId = ticketTypeIdB,
                    TicketPriorityId = ticketPriorityIdU,
                    AssignedUserId = assignedId1
                });
            }
            //ticket seed 2
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 2, P1"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 2, P1",
                    Description = "This is the second ticket for Demo project 1",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId1,
                    SubmitterId = submitterId1,
                    TicketStatusId = ticketStatusIdW,
                    TicketTypeId = ticketTypeIdB,
                    TicketPriorityId = ticketPriorityIdH,
                    AssignedUserId = assignedId2
                });
            }
            //ticket seed 3
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 3, P1"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 3, P1",
                    Description = "This is the third ticket for Demo project 1",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId1,
                    SubmitterId = submitterId2,
                    TicketStatusId = ticketStatusIdQ,
                    TicketTypeId = ticketTypeIdB,
                    TicketPriorityId = ticketPriorityIdM,
                    AssignedUserId = assignedId3
                });
            }
            //ticket seed 4
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 4, P1"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 4, P1",
                    Description = "This is the third ticket for Demo project 1",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId1,
                    SubmitterId = submitterId2,
                    TicketStatusId = ticketStatusIdQ,
                    TicketTypeId = ticketTypeIdB,
                    TicketPriorityId = ticketPriorityIdN,
                    AssignedUserId = assignedId4
                });
            }

            //*****************ENHANCEMENT TYPE TICKETS*******************
            //************************PROJECT #2**************************

            //ticket seed 1
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 1, P2"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 1, P2",
                    Description = "This is the first ticket for Demo project 2",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId2,
                    SubmitterId = submitterId1,
                    TicketStatusId = ticketStatusIdN,
                    TicketTypeId = ticketTypeIdE,
                    TicketPriorityId = ticketPriorityIdU,
                    AssignedUserId = assignedId1
                });
            }
            //ticket seed 2
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 2, P2"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 2, P2",
                    Description = "This is the second ticket for Demo project 2",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId2,
                    SubmitterId = submitterId1,
                    TicketStatusId = ticketStatusIdW,
                    TicketTypeId = ticketTypeIdE,
                    TicketPriorityId = ticketPriorityIdH,
                    AssignedUserId = assignedId2
                });
            }
            //ticket seed 3
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 3, P2"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 3, P2",
                    Description = "This is the third ticket for Demo project 2",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId2,
                    SubmitterId = submitterId2,
                    TicketStatusId = ticketStatusIdQ,
                    TicketTypeId = ticketTypeIdE,
                    TicketPriorityId = ticketPriorityIdM,
                    AssignedUserId = assignedId3
                });
            }
            //ticket seed 4
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 4, P2"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 4, P2",
                    Description = "This is the third ticket for Demo project 2",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId2,
                    SubmitterId = submitterId2,
                    TicketStatusId = ticketStatusIdQ,
                    TicketTypeId = ticketTypeIdE,
                    TicketPriorityId = ticketPriorityIdN,
                    AssignedUserId = assignedId4
                });
            }

            //*****************DESIGN FEATURE TICKET*********************
            //**********************PROJECT #3***************************
            //ticket seed 1
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 1, P3"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 1, P3",
                    Description = "This is the first ticket for Demo project 3",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId3,
                    SubmitterId = submitterId1,
                    TicketStatusId = ticketStatusIdN,
                    TicketTypeId = ticketTypeIdD,
                    TicketPriorityId = ticketPriorityIdU,
                    AssignedUserId = assignedId1
                });
            }
            //ticket seed 2
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 2, P3"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 2, P3",
                    Description = "This is the second ticket for Demo project 3",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId3,
                    SubmitterId = submitterId1,
                    TicketStatusId = ticketStatusIdW,
                    TicketTypeId = ticketTypeIdD,
                    TicketPriorityId = ticketPriorityIdH,
                    AssignedUserId = assignedId2
                });
            }
            //ticket seed 3
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 3, P3"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 3, P3",
                    Description = "This is the third ticket for Demo project 3",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId3,
                    SubmitterId = submitterId2,
                    TicketStatusId = ticketStatusIdQ,
                    TicketTypeId = ticketTypeIdD,
                    TicketPriorityId = ticketPriorityIdM,
                    AssignedUserId = assignedId3
                });
            }
            //ticket seed 4
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 4, P3"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 4, P3",
                    Description = "This is the third ticket for Demo project 3",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId3,
                    SubmitterId = submitterId2,
                    TicketStatusId = ticketStatusIdQ,
                    TicketTypeId = ticketTypeIdD,
                    TicketPriorityId = ticketPriorityIdN,
                    AssignedUserId = assignedId4
                });
            }

            //********************UNKNOWN TICKET*********************
            //**********************PROJECT #4***********************
            //ticket seed 1
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 1, P4"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 1, P4",
                    Description = "This is the first ticket for Demo project 4",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId4,
                    SubmitterId = submitterId1,
                    TicketStatusId = ticketStatusIdN,
                    TicketTypeId = ticketTypeIdU,
                    TicketPriorityId = ticketPriorityIdU,
                    AssignedUserId = assignedId1
                });
            }
            //ticket seed 2
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 2, P4"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 2, P4",
                    Description = "This is the second ticket for Demo project 4",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId4,
                    SubmitterId = submitterId1,
                    TicketStatusId = ticketStatusIdW,
                    TicketTypeId = ticketTypeIdU,
                    TicketPriorityId = ticketPriorityIdH,
                    AssignedUserId = assignedId2
                });
            }
            //ticket seed 3
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 3, P4"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 3, P4",
                    Description = "This is the third ticket for Demo project 4",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId4,
                    SubmitterId = submitterId2,
                    TicketStatusId = ticketStatusIdQ,
                    TicketTypeId = ticketTypeIdU,
                    TicketPriorityId = ticketPriorityIdM,
                    AssignedUserId = assignedId3
                });
            }
            //ticket seed 4
            if (!context.Tickets.Any(t => t.Title == "Test Ticket 4, P4"))
            {
                context.Tickets.Add(new Models.Ticket
                {
                    Title = "Test Ticket 4, P4",
                    Description = "This is the third ticket for Demo project 4",
                    Created = new DateTimeOffset(DateTime.Now),
                    ProjectId = projectId4,
                    SubmitterId = submitterId2,
                    TicketStatusId = ticketStatusIdQ,
                    TicketTypeId = ticketTypeIdU,
                    TicketPriorityId = ticketPriorityIdN,
                    AssignedUserId = assignedId4
                });
            }
            context.SaveChanges();

        }
    }
}
