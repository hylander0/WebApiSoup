using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiSoup.Repository
{
    public class RepoContext : DbContext
    {
        public RepoContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<SoupRecipe> SoupRecipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Configurations.UserProfileConfiguration());
            modelBuilder.Configurations.Add(new Configurations.ApplicationConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}