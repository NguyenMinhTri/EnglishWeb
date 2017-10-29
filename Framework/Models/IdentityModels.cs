using Framework.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Framework.Models
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, ApplicationUserRole, IdentityUserClaim>
    {
        public ApplicationDbContext()
            : base("DbConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(FrameworkContext.FrameworkDbContext.schema);
        }
    }
}