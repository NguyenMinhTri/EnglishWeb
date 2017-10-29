using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model
{
    public class ApplicationUserStore :
        UserStore<ApplicationUser, ApplicationRole, string, IdentityUserLogin, ApplicationUserRole, IdentityUserClaim>,
        IUserStore<ApplicationUser>,
        IUserStore<ApplicationUser, string>, IDisposable
    {
        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }
}
