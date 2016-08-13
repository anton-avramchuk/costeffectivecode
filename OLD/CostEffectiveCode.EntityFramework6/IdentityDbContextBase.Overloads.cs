using System.Data.Common;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CostEffectiveCode.EntityFramework6
{
    public class IdentityDbContextBase<TUser> : IdentityDbContextBase<TUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
        where TUser : IdentityUser
    {
        #region ctors

        public IdentityDbContextBase()
        {
        }

        public IdentityDbContextBase(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public IdentityDbContextBase(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
        }

        public IdentityDbContextBase(DbCompiledModel model) : base(model)
        {
        }

        public IdentityDbContextBase(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public IdentityDbContextBase(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }
        #endregion

    }

    public class IdentityDbContextBase : IdentityDbContextBase<IdentityUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        #region ctors
        public IdentityDbContextBase()
        {
        }

        public IdentityDbContextBase(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public IdentityDbContextBase(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
        }

        public IdentityDbContextBase(DbCompiledModel model) : base(model)
        {
        }

        public IdentityDbContextBase(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
        }

        public IdentityDbContextBase(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
        }
        #endregion
    }
}
