using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiSoup.Repository
{
    public class RecipeIngredient
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid IngredientId { get; set; }
        public String Name { get; set; }
        public String Quantity { get; set; }
        public virtual SoupRecipe Recipe { get; set; }
    }
}