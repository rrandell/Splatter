using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Splatter.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private UserManager<ApplicationUser> usermanager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(
                new ApplicationDbContext()));

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //These are all helper functions to assigning roles
        public bool AddUserToRole(string roleName)
        {
            var idResult = usermanager.AddToRole(this.Id, roleName);
            return idResult.Succeeded;
        }

        public bool RemoveUserFromRole(string roleName)
        {
            var idResult = usermanager.RemoveFromRole(this.Id, roleName);
            return idResult.Succeeded;
        }

        public bool IsInRole(string roleName)
        {
            //pass it the id of the current user
            var idResult = usermanager.IsInRole(this.Id, roleName);
            return idResult;
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}