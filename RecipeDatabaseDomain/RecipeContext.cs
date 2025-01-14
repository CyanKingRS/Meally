﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RecipeDatabaseDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabaseDomain
{
    public class RecipeContext : DbContext
    {
        public RecipeContext(DbContextOptions<RecipeContext> options)
           : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<UserIngredient> UserIngredients { get; set; }

        public DbSet<Label> Labels { get; set; }

        public DbSet<LabelRecipe> LabelRecipes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MeallyRecipeDatabase;Trusted_Connection=True;");

            }
        }
    }
}
