using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity; //where applicationDBcontext is found
using Splatter.Models;

namespace Splatter.Views.Roles
{
    public class RolesController : Controller
    {
        // GET: Roles   This is a list of the roles
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            //Get a list of roles out of the DB
            ApplicationDbContext db = new ApplicationDbContext();
            //Takes everyting in the roles portion of the database put it in a list and assigning it to the variable roles.
            var roles = db.Roles.ToList();
            //Create a list of Roles
            var model = new List<RolesViewModel>();
            //take information out of roles list in db and put it into our model-anytime going through contents of list For Each loop
            foreach (var item in roles)
            {
                //This is our model constructed Loops through the database and gets the role id and role name
                model.Add(new RolesViewModel { RoleId = item.Id, RoleName = item.Name });
            }
            
            return View(model);
        }


        // GET: Roles/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult CreateRole()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                //Builds an idenity called rolename and and sends it to the Roles in the Db, get back the role object from the role id
                var result = db.Roles.Add(new IdentityRole(model.RoleName));
                //checking to make sure that the results are returning a value
                if (result != null)
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            //If were here something is wrong
            return View(model);
        }


        // GET: Roles/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult EditRole(string id)
        {
            //get to the DB
            ApplicationDbContext db = new ApplicationDbContext();
            //locate the role and get it
            var role = db.Roles.Find(id);
            //build the view model
            var model = new RolesViewModel();// or {RoleId = id, RoleName = role.Name} after the Rolesviewmodel
            model.RoleId = id;
            model.RoleName = role.Name;
            //send the model to the view
            return View(model);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(RolesViewModel model)
        {
            //get to the db
            ApplicationDbContext db = new ApplicationDbContext();
            //locate the roll and get it.  we have a role Id in the model
            var role = db.Roles.Find(model.RoleId);
            //change the role name to match whats in the model
            role.Name = model.RoleName;
            //tell the DB the role entry has been modified
            db.Entry(role).State = EntityState.Modified;
            //save the changes
            db.SaveChanges();
            //Send us back to the list
            return RedirectToAction("Index");
        }

        // GET: Roles/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteRole(string id)
        {
            //
            //get to the DB
            ApplicationDbContext db = new ApplicationDbContext();
            //locate the role and get it
            var role = db.Roles.Find(id);
            //build the view model
            var model = new RolesViewModel();// or {RoldId = id, RoleName = role.Name} after the Rolesviewmodel
            model.RoleId = id;
            model.RoleName = role.Name;
            //send the model to the view
            return View(model);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRole(RolesViewModel model)
        {
            //get to the DB
            ApplicationDbContext db = new ApplicationDbContext();
            //locate the role and get it  Find returns an Identity Role
            var role = db.Roles.Find(model.RoleId);
            //remove the role from the DB
            db.Roles.Remove(role);
            //Save
            db.SaveChanges();
            //Redirect control back to the roles list view
            return RedirectToAction("Index");
        }

        //// GET: Roles/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}
    }
}
