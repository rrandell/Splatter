using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Splatter.Models
{
    public class UserViewModel
    {
        public string aspnetuserId;

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email / Login")]
        public string FullName { get; set; }

        //[Display(Name = "User Roles")]
        //public string FullName { get; set; }

        //[Display(Name = "Assign Project")]
        //public string FullName { get; set; }

        //[Display(Name = "Email / Login")]
        //public string FullName { get; set; }


    }
}