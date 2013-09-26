using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSoup.Contracts
{
    public class Ingredient
    {
        public Guid IngredientId { get; set; }
        public String Name { get; set; }
        public String Quantity { get; set; }
        public Recipe Recipe { get; set; }

    }
}