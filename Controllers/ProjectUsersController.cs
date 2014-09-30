using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Splatter.Models;

namespace Splatter.Controllers
{
    public class ProjectUsersController : Controller
    {
        private BugTrackerEntities BtDb = new BugTrackerEntities();

        // GET: AssingUsers
        [Authorize(Roles = "Administrator")]
        public ActionResult AssignUsers(int id)
        {
            var model = new ProjectUsersViewModel { ProjectId = id, ProjectName = BtDb.Projects.Find(id).Name };
            List<BTUser> userlist = new List<BTUser>();

            foreach (var user in BtDb.BTUsers.ToList())
            {
                if (!user.IsOnProject(id))
                {
                    userlist.Add(user);                        
                }
            }
            
            model.Users = new MultiSelectList(userlist, "UserName", "FirstName", null);
            return View(model);
        }

        // Post: AssignUsers
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult AssignUsers(ProjectUsersViewModel model)
        {
             if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string username in model.SelectedUsers)
                    {
                        //locate the user in the DB (BTUsers)
                        var user = BtDb.BTUsers.FirstOrDefault(a => a.UserName == username);
                        //add user to the Project
                        user.AddUserToProject(model.ProjectId);
                       
                    }
                }
                //redirect to the roles list
                return RedirectToAction("Index", "Projects");
            }
            //if we got here there is a problem
            return View(model);
        }
        

        // GET: UsersOnProject
        [Authorize(Roles = "Administrator")]
        public ActionResult UsersOnProject(int id)
        {
            var model = new ProjectUsersViewModel { ProjectId = id, ProjectName = BtDb.Projects.Find(id).Name };
            List<BTUser> userlist = new List<BTUser>();

            foreach (var user in BtDb.BTUsers.ToList())
            {
                if (user.IsOnProject(id))
                {
                    userlist.Add(user);
                }

            }
            model.Users = new MultiSelectList(userlist, "UserName", "FirstName", null);
            return View(model);
        }

        // POST: UsersOnProject
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult UsersOnProject(ProjectUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string username in model.SelectedUsers)
                    {
                        //locate the user in the DB (BTUsers)
                        var user = BtDb.BTUsers.FirstOrDefault(a => a.UserName == username);
                        //add user to the Project
                        user.RemoveUserFromProject(model.ProjectId);

                    }
                }
                //redirect to the roles list
                return RedirectToAction("Index", "Projects");
            }
            //if we got here there is a problem
            return View(model);
        }

       
    }
}
