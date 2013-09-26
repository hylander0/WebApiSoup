using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApiSoup.Common;
using WebMatrix.WebData;

namespace WebApiSoup.Repository.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Repository.RepoContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Repository.RepoContext context)
        {
            if (!WebMatrix.WebData.WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }

            Migrations.Setup.ConfigureSecurityRolesIfNotExist();

            if (!WebSecurity.UserExists("admin@admin.com"))
            {
                WebSecurity.CreateUserAndAccount("admin@admin.com", "admin1");
            }

            if (!Roles.GetRolesForUser("admin@admin.com").Contains(Constants.ROLES_ADMINISTRATOR))
            {
                Roles.AddUsersToRoles(new[] { "admin@admin.com" }, new[] { Constants.ROLES_ADMINISTRATOR });
            }

            //Add Initial User
            int userId = WebSecurity.GetUserId("admin@admin.com");
            Repository.UserProfile profile = context.UserProfiles
                                                .Where(w => w.UserId == userId)
                                                .FirstOrDefault();
            if (profile.Applications.Count == 0)
            {
                profile.Applications.Add(new Application()
                {
                    ApiAccessMetricLimit = Common.Constants.API_ACCESS_METRIC_NO_LIMIT,
                    SubscriptionType = Common.Constants.API_SUBSCRIPTION_TYPE_NO_LIMIT,
                    CreateDtm = DateTime.UtcNow,
                    LastDtmAccessed = DateTime.UtcNow,
                    AppGeneratedSecret = "EIbl7vLiCHphVhUJDDh78c7scVPLwJNnM00J",//Security.HashSecurity.GenerateAppSecret(),
                    AppGivenName = "First App"
                });
                context.SaveChanges();
            }
            Setup.SeedSoupDataIfNotExists(context);



        }


    }
}