using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Splatter.Models;
using PagedList.Mvc;

namespace Splatter.Controllers
{
    public class TicketsController : Controller
    {
        private BugTrackerEntities db = new BugTrackerEntities();
        private ApplicationDbContext AppDb = new ApplicationDbContext();

        private static int ProjectId = 0;

        private static bool SortDirection;
        private static string SortProperty;

        private static IEnumerable<Ticket> tickets;

        public ActionResult Sort(string property, IEnumerable<Ticket> model)
        {
            tickets = model;

            if (SortProperty == property)
            {
                SortDirection = !SortDirection;
            }
            else
            {
                SortDirection = false;
            }
            SortProperty = property;           
            return RedirectToAction("Index");
        }

        // GET: Tickets
        [Authorize(Roles = "Administrator, Developer, Submitter, Guest")]
        public ActionResult Index(int? projectid, string param, string paramtype, int? page)
        {
            if (projectid != null) 
            {
                ProjectId = (int)projectid;
            }

            var tickets = db.Tickets.Include(t => t.BTUser).Include(t => t.BTUser1).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            if (ProjectId > 0 )
            {
                ViewBag.pid = ProjectId;
                ViewBag.Title = "Tickets:" + db.Projects.Find(ProjectId).Name;
                tickets = tickets.Where(t => t.ProjectId == ProjectId);                
            }
            else
	        {
                ViewBag.Title = "Tickets";
	        }

            switch (paramtype)
            {
                case "Owner":
                    tickets = tickets.Where(t => t.BTUser.FirstName == param);
                    break;
                case "Assigned To":
                    tickets = tickets.Where(t => t.BTUser1.FirstName == param);
                    break;
                case "Project":
                    tickets = tickets.Where(t => t.Project.Name == param);
                    break;
                case "Priority":
                    tickets = tickets.Where(t => t.TicketPriority.Name == param);
                    break;
                case "Status":
                    tickets = tickets.Where(t => t.TicketStatus.Name == param);
                    break;
                case "Type":
                    tickets = tickets.Where(t => t.TicketType.Name == param);
                    break;
                default:
                    break;
            }

		        switch (SortProperty)
                {
                    case "Title":
                        if (!SortDirection)
                        {
                            tickets = tickets.OrderBy(t => t.Title);
                        }
                        else
                        {
                            tickets = tickets.OrderByDescending(t => t.Title);
                        }
                        break;
                    case "Last Updated":
                        if (!SortDirection)
                        {
                            tickets = tickets.OrderBy(t => t.Updated);
                        }
                        else
                        {
                            tickets = tickets.OrderByDescending(t => t.Updated);
                        }
                        break;
                    case "Owner":
                        if (!SortDirection)
                        {
                            tickets = tickets.OrderBy(t => t.OwnerUserName);
                        }
                        else
                        {
                            tickets = tickets.OrderByDescending(t => t.OwnerUserName);
                        }
                        break;
                    case "Assigned To":
                        if (!SortDirection)
                        {
                            tickets = tickets.OrderBy(t => t.AssignedToUserName);
                        }
                        else
                        {
                            tickets = tickets.OrderByDescending(t => t.AssignedToUserName);
                        }
                        break;
                    case "Project":
                        if (!SortDirection)
                        {
                            tickets = tickets.OrderBy(t => t.Project.Name);
                        }
                        else
                        {
                            tickets = tickets.OrderByDescending(t => t.Project.Name);
                        }
                        break;
                    case "Priority":
                        if (!SortDirection)
                        {
                            tickets = tickets.OrderBy(t => t.PriorityId);
                        }
                        else
                        {
                            tickets = tickets.OrderByDescending(t => t.PriorityId);
                        }
                        break;
                    case "Status":
                        if (!SortDirection)
                        {
                            tickets = tickets.OrderBy(t => t.StatusId);
                        }
                        else
                        {
                            tickets = tickets.OrderByDescending(t => t.StatusId);
                        }
                        break;
                    case "Type":
                        if (!SortDirection)
                        {
                            tickets = tickets.OrderBy(t => t.TypeId);
                        }
                        else
                        {
                            tickets = tickets.OrderByDescending(t => t.TypeId);
                        }
                        break;

                    default:
                        {
                            tickets = tickets.OrderBy(t => t.Created);
                            break;
                        }
                }

            int pagesize = 10;
            int pagenumber = (page ?? 1);

            if (User.IsInRole("Administrator"))
            {
                 return View(tickets.ToPagedList(pagenumber, pagesize));
            }
            else if (User.IsInRole("Developer"))
            {
                return View(tickets.Where(t => t.AssignedToUserName == User.Identity.Name).ToPagedList(pagenumber, pagesize));
            }
            else 
            {
                return View(tickets.Where(t => t.OwnerUserName == User.Identity.Name).ToPagedList(pagenumber, pagesize)); 
            }
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Administrator, Developer, Submitter,Guest")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ViewBag.pid = ProjectId;
            Ticket ticket = db.Tickets.Find(id);
            ViewBag.OwnerUserName = new SelectList(db.BTUsers, "UserName", "DisplayName", User.Identity.Name);
            ViewBag.AssignedToUserName = new SelectList(db.BTUsers, "UserName", "DisplayName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.Project.Name);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.PriorityId);
            ViewBag.StatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter, Guest")]
        public ActionResult Create()
        {
            Ticket model = new Ticket();
            DateTimeOffset date = DateTimeOffset.Now;
            model.Created = date;
            model.Updated = date;
            model.OwnerUserName = User.Identity.Name;

            ViewBag.pid = ProjectId;
            ViewBag.OwnerUserName = new SelectList(db.BTUsers, "UserName", "DisplayName", User.Identity.Name);
            ViewBag.AssignedToUserName = new SelectList(db.BTUsers, "UserName", "DisplayName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", model.ProjectId);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", model.PriorityId);
            ViewBag.StatusId = new SelectList(db.TicketStatuses, "Id", "Name", model.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", model.TypeId);
            return View(model);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Submitter, Guest")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TypeId,PriorityId,StatusId,OwnerUserName,AssignedToUserName")] Ticket ticket)
        {
          
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerUserName = new SelectList(db.BTUsers, "UserName", "FirstName", ticket.OwnerUserName);
            ViewBag.AssignedToUserName = new SelectList(db.BTUsers, "UserName", "FirstName", ticket.AssignedToUserName);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.PriorityId);
            ViewBag.StatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Administrator, Developer, Submitter, Guest")]
        public ActionResult Edit(int? id)
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
            
            ticket.Updated = DateTimeOffset.Now;
            TempData["ticket"] = ticket;
            ViewBag.pid = ProjectId;
            ViewBag.OwnerUserName = new SelectList(db.BTUsers, "UserName", "FirstName", ticket.OwnerUserName);
            ViewBag.AssignedToUserName = new SelectList(db.BTUsers, "UserName", "FirstName", ticket.AssignedToUserName);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.PriorityId);
            ViewBag.StatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrator, Developer, Submitter, Guest")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TypeId,PriorityId,StatusId,OwnerUserName,AssignedToUserName")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //retrieve the old version of the ticket from the db
                var oldTicket = (Ticket)TempData["ticket"];
                //determine whats changed and for each changed property, add a new TicketHistory entry
                //to the DB and save the changes
                if (oldTicket.Description != ticket.Description)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Property = "Description",
                        Changed = DateTimeOffset.Now,
                        UserName = User.Identity.Name,
                        TicketId = ticket.Id,
                        OldValue = oldTicket.Description,
                        NewValue = ticket.Description
                    });
                }

                if (oldTicket.ProjectId != ticket.ProjectId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Property = "Project",
                        Changed = DateTimeOffset.Now,
                        UserName = User.Identity.Name,
                        TicketId = ticket.Id,
                        OldValue = oldTicket.Project.Name,
                        NewValue = db.Projects.Find(ticket.ProjectId).Name
                    });
                }

                if (oldTicket.TypeId != ticket.TypeId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Property = "TypeId",
                        Changed = DateTimeOffset.Now,
                        UserName = User.Identity.Name,
                        TicketId = ticket.Id,
                        OldValue = oldTicket.TicketType.Name,
                        NewValue = db.Projects.Find(ticket.TypeId).Name
                    });
                }

                if (oldTicket.PriorityId != ticket.PriorityId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Property = "TicketPriority",
                        Changed = DateTimeOffset.Now,
                        UserName = User.Identity.Name,
                        TicketId = ticket.Id,
                        OldValue = oldTicket.TicketPriority.Name,
                        NewValue = db.Projects.Find(ticket.PriorityId).Name
                    });
                }

                if (oldTicket.StatusId != ticket.StatusId)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Property = "Status",
                        Changed = DateTimeOffset.Now,
                        UserName = User.Identity.Name,
                        TicketId = ticket.Id,
                        OldValue = oldTicket.TicketStatus.Name,
                        NewValue = db.Projects.Find(ticket.StatusId).Name
                    });
                }

               if (oldTicket.AssignedToUserName != ticket.AssignedToUserName)
                {
                    db.TicketHistories.Add(new TicketHistory
                    {
                        Property = "Assigned To",
                        Changed = DateTimeOffset.Now,
                        UserName = User.Identity.Name,
                        TicketId = ticket.Id,
                        OldValue = oldTicket.AssignedToUserName,
                        NewValue = ticket.AssignedToUserName
                    });
                }
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerUserName = new SelectList(db.BTUsers, "UserName", "FirstName", ticket.OwnerUserName);
            ViewBag.AssignedToUserName = new SelectList(db.BTUsers, "UserName", "FirstName", ticket.AssignedToUserName);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.PriorityId);
            ViewBag.StatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
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
