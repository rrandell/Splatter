using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Splatter.Models.EntityAddOns
{
    //public partial class BTUser
    //{
    //    public string UserName { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string DisplayName { get; set; }
    //    public string AspNetUserId { get; set; }

    //    public bool IsOnProject(int id)
    //    {
    //        BugTrackerEntities db = new BugTrackerEntities();

    //        foreach (var user in db.ProjectUsers)
    //        {
    //            if (user.ProjectId == id && user.UserName == this.UserName)
    //            {
    //                return true;

    //            }
    //        }
    //            return false;
    //     }
        

    //    public void AddUserToProject(int id)
    //    {
    //        BugTrackerEntities db = new BugTrackerEntities();
    //        if (!this.IsOnProject(id))
    //        {
    //            db.ProjectUsers.Add(new ProjectUser { ProjectId = id, UserName = this.UserName });
    //            db.SaveChanges();
    //        }
    //    }

    //    public void RemoveUserFromProject(int id)
    //    {
    //        BugTrackerEntities db = new BugTrackerEntities();
    //        if (this.IsOnProject(id))
    //        {
    //            foreach (var user in db.ProjectUsers)
    //            {
    //                if (user.ProjectId == id && user.UserName == this.UserName)
    //                {
    //                    db.ProjectUsers.Remove(user);
    //                    break;
    //                }
    //            }
    //            db.SaveChanges();
    //        }
    //    }
    //}
}