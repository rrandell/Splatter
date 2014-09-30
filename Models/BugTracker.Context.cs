﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Splatter.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BugTrackerEntities : DbContext
    {
        public BugTrackerEntities()
            : base("name=BugTrackerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BTUser> BTUsers { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }
        public virtual DbSet<TicketAttachment> TicketAttachments { get; set; }
        public virtual DbSet<TicketComment> TicketComments { get; set; }
        public virtual DbSet<TicketHistory> TicketHistories { get; set; }
        public virtual DbSet<TicketNotification> TicketNotifications { get; set; }
        public virtual DbSet<TicketPriority> TicketPriorities { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketStatus> TicketStatuses { get; set; }
        public virtual DbSet<TicketType> TicketTypes { get; set; }
    }
}
