using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace WebApiSoup.Repository.Configurations
{
    public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>

    {

        public UserProfileConfiguration() : base()
        {
            //UserProfile has many applications, that is also optional (pointing to other side of relationship)
            //HasMany(p => p.Applications)
              //  .WithRequired(s => s.UserProfile)
              //  .WillCascadeOnDelete(true);


        }
    }
}