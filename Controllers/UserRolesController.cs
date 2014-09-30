using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Splatter.Models;

namespace Splatter.Views.UserRoles
{
    public class UserRolesController : Controller
    {
        private ApplicationDbContext AspDb = new ApplicationDbContext();
        private BugTrackerEntities BtDb = new BugTrackerEntities();

        //Get: AssingUsers  the name of the method is always the same as the view
        [Authorize(Roles = "Administrator")]
        public ActionResult AssignUsers(string id)
        {
            //locate the role in the DB
            var role = AspDb.Roles.Find(id);
            //build the view model
            var model = new UserRolesViewModel();
            //add the role ID and Name to the model
            model.RoleId = role.Id;
            model.RoleName = role.Name;
            //instantiate the user list that is part of the view model.  This is what populates the Multiselect list
            List<BTUser> userlist = new List<BTUser>();
            //loop through the system users(foreach loop) and, as long as the user is NOT alread in the role,
            //   add the user to the list
            foreach (var user in AspDb.Users)
            {
                //if the user is not already in the role
                if (!user.IsInRole(model.RoleName))
                {
                    //  go to the bug tracker database and go to the table in the db find the first db entry where the users aspnet data field matches the current user in the loop.
                    //find it and set it in the temp user and add it to our user list            
                    //Every time we find a user in that list that is not assigned we populate it to the list
                    //
                    //Can also be written this way
                    userlist.Add(BtDb.BTUsers.FirstOrDefault(u => u.AspNetUserId == user.Id));
                    
                }
            }
            //instansiate the MultiSelectList object (in the model) using the newly built list
            //   with appropriate value and display parameters (Hint: new MultiSelectList(Users.ToList(), "ASPNETUSERS", DisplayName
            model.Users = new MultiSelectList(userlist, "AspNetUserId", "DisplayName", null);
            //send the model to the view
            return View(model);
        }
        //Post:  AssingUsers
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult AssignUsers(UserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string id in model.SelectedUsers)
                    {
                        //locate the user in the DB (AspNetUsers)
                        var user = AspDb.Users.Find(id);
                        //add user to the role
                        user.AddUserToRole(model.RoleName);
                        //if the user is in the Unassigned role, remove from that role
                        if (user.IsInRole("Unassigned"))
                        {
                            user.RemoveUserFromRole("Unassigned");
                        }
                    }
                }
                //redirect to the roles list
                return RedirectToAction("Index", "Roles");
            }
            //if we got here there is a problem
            return View(model);
        }

        //Get: UsersInRole
        [Authorize(Roles = "Administrator")]
        public ActionResult UsersInRole(string id)
        {
            //locate the role in the DB
            var role = AspDb.Roles.Find(id);
            //build the view model
            var model = new UserRolesViewModel();
            //add the role ID and Name to the model
            model.RoleId = role.Id;
            model.RoleName = role.Name;
            //instantiate the user list that is part of the view model
            List<BTUser> userlist = new List<BTUser>();
            //loop through the system users(foreach loop) and, as long as the user is NOT alread in the role,
            //   add the user to the list
            foreach (var user in AspDb.Users)
            {
                //if the user is in the role
                if (user.IsInRole(model.RoleName))
                {
                    //  go to the bug tracker database and go to the table in the db find the first db entry where the users aspnet data field matches the current user in the loop.
                    //find it and set it in the temp user and add it to our user list            
                    //Every time we find a user in that list that is not assigned we populate it to the list
                    userlist.Add(BtDb.BTUsers.FirstOrDefault(u => u.AspNetUserId == user.Id));
                    //Can also be written this way
                    // var tempuser = btDb.BTUsers.FirstorDefault(u=> u.AspNetUserId == user.Id);
                    //userlist.Add(tempuser);
                }
            }
            //instansiate the MultiSelectList object (in the model) using the newly built list
            //   with appropriate value and display parameters (Hint: new MultiSelectList(Users.ToList(), "ASPNETUSERS", DisplayName
            model.Users = new MultiSelectList(userlist, "AspNetUserId", "DisplayName", null);
            //send the model to the view
            return View(model);
        }

        //Post: UsersInRole
        [HttpPost]
        //line for when only the administrator can use the feature
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult UsersInRole(UserRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.SelectedUsers != null)
                {
                    foreach (string id in model.SelectedUsers)
                    {
                        //locate the user in the DB (AspNetUsers)
                        var user = AspDb.Users.FirstOrDefault(u => u.Id == id);
                        
                        user.RemoveUserFromRole(model.RoleName);
                        
                        if (user.Roles.Count == 0)
                        {
                           user.AddUserToRole("Submitter");
                        }
                    }
                }
                //redirect to the roles list
                return RedirectToAction("Index", "Roles");
            }
            //if we got here there is a problem
            return View(model);
        }

    }
}    

