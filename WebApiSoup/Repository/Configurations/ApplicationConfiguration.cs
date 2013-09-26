using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace WebApiSoup.Repository.Configurations
{
    public class ApplicationConfiguration : EntityTypeConfiguration<Application>
    {
        public ApplicationConfiguration()
            : base()
        {
            //Object must contain a UserProfile
           //HasRequired(hr => hr.UserProfile);
        }
    }
}