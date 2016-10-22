namespace AspNetIdentityWithAngularjs.Migrations
{
    using Data.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AspNetIdentityWithAngularjs.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Asp.NetIdentityWithAngularjs.Data.ApplicationDbContext";
        }

        protected override void Seed(AspNetIdentityWithAngularjs.Data.ApplicationDbContext context)
        {

            ApplicationUserManager userMgr = new ApplicationUserManager(new UserStore<IdentityUser>(context));            

            //string roleName = "Administrators";
            string userName = "Admin";
            string password = "Secret";
            string email = "admin@example.com";

            //if (!roleMgr.RoleExists(roleName))
            //{
            //    roleMgr.Create(new StoreRole(roleName));
            //}

            IdentityUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(new IdentityUser
                {
                    UserName = userName,
                    Email = email
                }, password);
                user = userMgr.FindByName(userName);
            }

            //if (!userMgr.IsInRole(user.Id, roleName))
            //{
            //    userMgr.AddToRole(user.Id, roleName);
            //}

            base.Seed(context);

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
