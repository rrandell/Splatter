using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Splatter.Models
{
    public class ProjectUsersViewModel
    {
            public int ProjectId { get; set; }

            [Display(Name = "Project Name")]
            public string ProjectName { get; set; }

            [Display(Name = "Users")]
            public System.Web.Mvc.MultiSelectList Users { get; set; }

            public string[] SelectedUsers { get; set; }

            //When we build a list box We have to define it as a Mulitselectlist
            //setting up the array selectedusers gets the users that we need to pass into the list box

        }
    }
