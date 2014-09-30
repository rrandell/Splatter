using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Splatter.Models
{
    public class RolesViewModel
    {
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public string RoleId { get; set; }

    }
}