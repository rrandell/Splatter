using Microsoft.Owin;
using Owin;
using Splatter.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

using PagedList.Mvc;

[assembly: OwinStartupAttribute(typeof(Splatter.Startup))]
namespace Splatter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
         }
    }
  
}
