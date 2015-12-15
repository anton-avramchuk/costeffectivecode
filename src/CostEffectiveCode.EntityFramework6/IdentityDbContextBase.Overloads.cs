using Microsoft.AspNet.Identity.EntityFramework;

namespace CostEffectiveCode.EntityFramework6
{
    public class IdentityDbContextBase<TUser> : IdentityDbContextBase<TUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
        where TUser : IdentityUser
    {
    }

    public class IdentityDbContextBase : IdentityDbContextBase<IdentityUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
    }
}
