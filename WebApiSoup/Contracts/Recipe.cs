using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSoup.Contracts
{
    public class Recipe
    {
        public Recipe()
        {
            Ingredients = new List<Ingredient>();
        }


        public Guid RecipeId { get; set; }
        public String RecipeName { get; set; }
        public String PrepTime { get; set; }
        public String PrepInstructions { get; set; }
        public String EquipmentRequired { get; set; }
        public String NumOfServings { get; set; }
        public String CookTime { get; set; }
        public String CookTemp { get; set; }
        public String FinishInstructions { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}