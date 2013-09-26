using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using WebApiSoup.Pipeline;

namespace WebApiSoup.Controllers
{
    public class SoupRecipesController : ApiController
    {
        [HttpGet]
        [ActionName("PerformHandshake")]
        //[CustomAuthorizeAttribute]
        public Contracts.HandshakeResponse PerformHandshake()
        {
            return new Contracts.HandshakeResponse()
            {
                IsHandshakeSuccessful = true
            };
        }
        [HttpGet]
        [ActionName("GetAllRecipes")]
        public List<Contracts.Recipe> GetAllRecipes()
        {
            List<Contracts.Recipe> retval = new List<Contracts.Recipe>();
            using(var context = new Repository.RepoContext())
            {
                List<Repository.SoupRecipe> DbRecipe = context.SoupRecipes.ToList();
                //MAP from DB object to API Contract
                DbRecipe.ForEach(f => {
                    Contracts.Recipe recipe = new Contracts.Recipe()
                    {
                        RecipeId = f.RecipeId,
                        RecipeName = f.RecipeName,
                        PrepTime = f.PrepTime,
                        PrepInstructions = f.PrepInstructions,
                        NumOfServings = f.NumOfServings,
                        FinishInstructions = f.FinishInstructions,
                        EquipmentRequired = f.EquipmentRequired,
                        CookTime = f.CookTime,
                        CookTemp = f.CookTemp
                    };
                    foreach (var dbIngredient in f.Ingredients)
                    {
                        recipe.Ingredients.Add(new Contracts.Ingredient()
                        {
                            IngredientId = dbIngredient.IngredientId,
                            Name = dbIngredient.Name,
                            Quantity = dbIngredient.Quantity
                        });
                    }

                    retval.Add(recipe);
                });

            }
            return retval;
        }
    }
}