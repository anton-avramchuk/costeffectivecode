using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace CostEffectiveCode.EntityFramework7
{
    public class IdentityDbContextBase<TUser> : IdentityDbContextBase<TUser, IdentityRole, string>
        where TUser : IdentityUser
    {
        public IdentityDbContextBase()
            : base()
        {
        }

        public IdentityDbContextBase(DbContextOptions options)
            : base(options)
        {
        }
    }

    public class IdentityDbContextBase : IdentityDbContextBase<IdentityUser, IdentityRole, string>
    {
        public IdentityDbContextBase(DbContextOptions options)
            : base(options)
        {
        }
    }
}
