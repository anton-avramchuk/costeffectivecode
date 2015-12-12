using Microsoft.AspNet.Identity.EntityFramework;

namespace CostEffectiveCode.EntityFramework7
{
    public class IdentityDbContextBase<TUser> : IdentityDbContextBase<TUser, IdentityRole, string>
        where TUser : IdentityUser
    {
    }

    public class IdentityDbContextBase : IdentityDbContextBase<IdentityUser, IdentityRole, string>
    {
    }
}
