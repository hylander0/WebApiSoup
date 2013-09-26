using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiSoup.Repository
{
    [Table("UserProfile")]
    public class UserProfile
    {
        public UserProfile()
        {
            Applications = new List<Application>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string EmailAddress { get; set;}
        public DateTime? CreateDtm { get; set;}

        public virtual ICollection<Application> Applications { get; set;}
    }
}
