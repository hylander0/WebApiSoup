using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiSoup.Repository
{
    public class SoupRecipe
    {
        public SoupRecipe()
        {
            Ingredients = new List<RecipeIngredient>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid RecipeId { get; set; }
        public String RecipeName { get; set; }
        public String PrepTime { get; set; }
        public String PrepInstructions { get; set; }
        public String EquipmentRequired { get; set; }
        public String NumOfServings { get; set; }
        public String CookTime { get; set; }
        public String CookTemp{ get; set; }
        public String FinishInstructions { get; set; }
        public virtual ICollection<RecipeIngredient> Ingredients { get; set; }

    }
}