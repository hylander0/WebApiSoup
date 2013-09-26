using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiSoup.Repository
{
    [Table("Application")]
    public class Application
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ApplicationId { get; set; }
        public String AppGivenName { get; set; }
        public String AppGeneratedSecret { get; set; }
        public String SubscriptionType { get; set; }
        public DateTime CreateDtm { get; set; }
        public DateTime LastDtmAccessed { get; set; }
        public int ApiAccessMetricLimit { get; set; }
        public int ApiAccessMetricCurrent { get; set; }

        public virtual UserProfile UserProfile { get; set; }

    }
}