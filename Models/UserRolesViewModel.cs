using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Splatter.Models
{
    public class UserRolesViewModel
    {
        public string RoleId { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Display(Name = "Users")]
        public System.Web.Mvc.MultiSelectList Users { get; set; }

        public string[] SelectedUsers { get; set; }

        //When we build a list box We have to define it as a Mulitselectlist
        //setting up the array selectedusers gets the users that we need to pass into the list box

    }
}