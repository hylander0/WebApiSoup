using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebApiSoup.Repository.Migrations
{
    public class Setup
    {
        public static void ConfigureSecurityRolesIfNotExist()
        {
            foreach (var role in Common.Constants.SystemRoles())
            {

                if (!Roles.RoleExists(role))
                {
                    Roles.CreateRole(role);
                }
            }
        }

        public static void SeedSoupDataIfNotExists(RepoContext context)
        {
            SoupRecipe recipe = context.SoupRecipes.Where(w => w.RecipeName == "French Onion Soup").FirstOrDefault();
            if (recipe == null)
            {
                //Source: http://www.foodnetwork.com/recipes/tyler-florence/french-onion-soup-recipe2/index.html
                recipe = new SoupRecipe()
                {
                    RecipeName = "French Onion Soup",
                    PrepTime = "15 min",
                    CookTime = "55 min",
                    PrepInstructions = @"Melt the stick of butter in a large pot over medium heat. Add the onions, garlic, bay leaves, thyme, and salt and pepper and cook until the onions are very soft and caramelized, about 25 minutes. Add the wine, bring to a boil, reduce the heat and simmer until the wine has evaporated and the onions are dry, about 5 minutes. Discard the bay leaves and thyme sprigs. Dust the onions with the flour and give them a stir. Turn the heat down to medium low so the flour doesn't burn, and cook for 10 minutes to cook out the raw flour taste. Now add the beef broth, bring the soup back to a simmer, and cook for 10 minutes. Season, to taste, with salt and pepper.

When you're ready to eat, preheat the broiler. Arrange the baguette slices on a baking sheet in a single layer. Sprinkle the slices with the Gruyere and broil until bubbly and golden brown, 3 to 5 minutes.

Ladle the soup in bowls and float several of the Gruyere croutons on top.

Alternative method: Ladle the soup into bowls, top each with 2 slices of bread and top with cheese. Put the bowls into the oven to toast the bread and melt the cheese. 
",
                    NumOfServings = "4 to 6",
                    Ingredients = new List<RecipeIngredient>()
                    {
                        new RecipeIngredient()
                        {
                            Name = "Unsalted butter",
                            Quantity = "1/2 cup"
                        },
                        new RecipeIngredient()
                        {
                            Name = "Onions, sliced",
                            Quantity = "4"
                        },
                        new RecipeIngredient()
                        {
                            Name = "garlic cloves, chopped",
                            Quantity = "2"
                        },
                        new RecipeIngredient()
                        {
                            Name = "bay leaves",
                            Quantity = "2"
                        },
                        new RecipeIngredient()
                        {
                            Name = "fresh thyme sprigs",
                            Quantity = "2"
                        },
                        new RecipeIngredient()
                        {
                            Name = "Kosher salt and freshly ground black pepper",
                            Quantity = "To Taste"
                        },
                        new RecipeIngredient()
                        {
                            Name = "red wine",
                            Quantity = "1/2 Cup"
                        },
                        new RecipeIngredient()
                        {
                            Name = "all-purpose flour",
                            Quantity = "3 heaping tablespoons "
                        },
                        new RecipeIngredient()
                        {
                            Name = "beef broth",
                            Quantity = "2 quarts"
                        },
                        new RecipeIngredient()
                        {
                            Name = "baguette, sliced",
                            Quantity = "1"
                        },
                        new RecipeIngredient()
                        {
                            Name = "grated Gruyere",
                            Quantity = "1/2 pound"
                        },
                    }
                    
                    
                };
                context.SoupRecipes.Add(recipe);
            }

            context.SaveChanges();
        }
    }
}